using Exsta_frontend;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => {
    HttpClient client = new() { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
    return client;
});

await builder.Build().RunAsync();
