using Microsoft.Azure.Cosmos;
using RideScheduler.Infrastructure;
using RideScheduler.Model;
using System.Text.Json;

namespace RideScheduler.Data
{
    public class CosmosDataProvider : IDataProvider
    {
        public CosmosDataProvider(IConfiguration config)
        {
            CosmosClient = new CosmosClient(config.GetConnectionString("CosmosUri"), config.GetConnectionString("CosmosKey"));
            InitializeAsync().Wait();
        }

        CosmosClient CosmosClient { get; }
        Database? Database { get; set; }
        Container? Riders { get; set; }
        Container? Trips { get; set; }

        async Task InitializeAsync()
        {
            Database = await CosmosClient.CreateDatabaseIfNotExistsAsync("RideScheduler");
            Riders = await Database.CreateContainerIfNotExistsAsync(nameof(Rider), $"/{nameof(Rider.Id)}");
            Trips = await Database.CreateContainerIfNotExistsAsync(nameof(Trip), $"/{nameof(Trip.Id)}");
        }

        public async Task CreateRiderAsync(Rider user)
        {
            throw new NotImplementedException();
        }

        public async Task<Rider?> GetRiderAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
