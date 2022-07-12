using Microsoft.Azure.Cosmos;
using RideScheduler.Infrastructure;
using RideScheduler.Model;
using System.Text.Json;

namespace RideScheduler.Cosmos
{
    public class CosmosDataProvider : DataProvider
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
            Riders = await Database.CreateContainerIfNotExistsAsync(nameof(Rider), $"/{nameof(Rider.GroupId)}");
            Trips = await Database.CreateContainerIfNotExistsAsync(nameof(Trip), $"/{nameof(Trip.RiderId)}");
        }

        protected override async Task CreateRiderAsync(Rider user)
        {
            throw new NotImplementedException();
        }

        protected override async Task<Rider?> GetRiderAsync(string username)
        {
            throw new NotImplementedException();
        }
    }
}
