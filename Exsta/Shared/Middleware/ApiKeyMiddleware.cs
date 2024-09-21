using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Middleware;

public class ApiKeyMiddleware {
    private readonly RequestDelegate _next;
    private const string APIKEYNAME = "x-api-key";

    public ApiKeyMiddleware(RequestDelegate next) {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context) {
        if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey)) {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        // Fetch valid API keys from your configuration or database
        var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = appSettings["user-service-api-key"] ?? throw new InvalidOperationException("No API Key was configured or no API Key could be found in configuration");

        if (!apiKey.Equals(extractedApiKey)) {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client.");
            return;
        }

        await _next(context);
    }
}
