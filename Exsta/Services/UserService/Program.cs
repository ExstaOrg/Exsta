using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Load CORS origins from configuration
var allowedCorsOrigins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string[]>()
    ?? [""];

builder.Services.AddCors(options => {
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins(allowedCorsOrigins) // Specify the allowed origins
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

//DbContext
builder.Services.AddDbContext<UserServiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserServiceSqlServer")));

// Add services to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName.Equals("LocalDevelopment")) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();
