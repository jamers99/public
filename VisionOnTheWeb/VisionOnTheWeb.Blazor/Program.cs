using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VisionOnTheWeb;
using VisionOnTheWeb.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
    .AddCors(options =>
    {
        options.AddPolicy(
            "Open",
            builder => builder.AllowAnyOrigin().AllowAnyHeader());
    });

await builder.Build().RunAsync();
