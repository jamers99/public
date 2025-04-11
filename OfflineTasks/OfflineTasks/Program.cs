using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OfflineTasks;
using OfflineTasks.MyEbms;
using OfflineTasks.Networking;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services
    .AddScoped<NetworkStatus>()
    .AddBlazoredLocalStorage()
    .AddMyEbms(o => o.Url = $"https://ecc101010101010101.servicebus.windows.net/MyEBMS/MEF/OData");

await builder.Build().RunAsync();
