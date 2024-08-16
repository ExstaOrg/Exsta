using Exsta_frontend;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Optional: Load environment-specific configuration (e.g., appsettings.Development.json)
builder.Configuration.AddJsonFile($"appsettings.{builder.HostEnvironment.Environment}.json", optional: true, reloadOnChange: true);

// Load the subscription key from configuration
string? subscriptionKey = builder.Configuration["ApiManagement:SubscriptionKey"];

Console.WriteLine("sub key:" + subscriptionKey);

builder.Services.AddScoped(sp => {
    HttpClient client = new() { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

    // Add the subscription key to the default request headers
    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

    return client;
});

await builder.Build().RunAsync();
