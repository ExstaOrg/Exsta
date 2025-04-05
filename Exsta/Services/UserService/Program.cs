using Backend_Shared.Application;
using Exsta_Shared.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Middleware;
using System.Text;
using UserService.Application;
using UserService.Data;
using UserService.Repositories;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsLocalDevelopment()) {
    builder.Configuration.AddUserSecrets<Program>();
}

// Load CORS origins from configuration
var allowedCorsOrigins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string[]>()
    ?? [""];

builder.Services.AddCors(options => {
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins(allowedCorsOrigins) // Specify the allowed origins
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// DbContext
// Prefer environment variable if available, fallback to appsettings
var sqlConnectionString = Environment.GetEnvironmentVariable("UserServiceSqlServer")
                      ?? builder.Configuration.GetConnectionString("UserServiceSqlServer")
                      ?? throw new NullReferenceException("No connection string configured for SQL server");
builder.Services.AddDbContext<UserServiceDbContext>(options =>
    options.UseSqlServer(sqlConnectionString));

Console.WriteLine($"SQL Connstring: {sqlConnectionString}");

// Add AppInsights
builder.Services.AddApplicationInsightsTelemetry(options => {
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

builder.Logging.AddConsole();  // Logs to Console
builder.Logging.AddDebug();    // Logs for Debugging
builder.Logging.AddApplicationInsights(); // Logs to App Insights

// Add services to the container.
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRegisterUserApplicationService, RegisterUserApplicationService>();
builder.Services.AddScoped<IPasswordApplicationService, PasswordApplicationService>(sp => {
    var pepper = Environment.GetEnvironmentVariable("passwordservice-pepper")
                    ?? builder.Configuration["passwordservice-pepper"]
                    ?? throw new NullReferenceException("Pepper is not configured.");
    return new PasswordApplicationService(pepper);
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    var securityScheme = new OpenApiSecurityScheme {
        Name = "JWT Authentication",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

    c.AddSecurityRequirement(securityRequirement);

    // Add API Key Authentication scheme
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme {
        Name = "x-api-key", // Header name
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Enter your API key"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] {}
        }
    });
});

builder.Services
    .AddAuthentication(x => {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x => {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("auth-service-private-key")
                                                                                ?? builder.Configuration["auth-service-private-key"]
                                                                                ?? throw new NullReferenceException("Private key was not initialized"))),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsLocalDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/api"), // Apply middleware only for API routes
    appBuilder => appBuilder.UseMiddleware<ApiKeyMiddleware>()
);

app.MapControllers();

app.Run();
