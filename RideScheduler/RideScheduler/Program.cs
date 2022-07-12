using RideScheduler.Cosmos;
using RideScheduler.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DataProvider>(new CosmosDataProvider(builder.Configuration));
builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Run();
