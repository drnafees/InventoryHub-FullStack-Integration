using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using InventoryHub.Client;
using InventoryHub.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000/") });
builder.Services.AddScoped<IInventoryApiClient, InventoryApiClient>();

await builder.Build().RunAsync();