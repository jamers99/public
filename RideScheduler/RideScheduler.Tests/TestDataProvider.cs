using RideScheduler.Infrastructure;
using RideScheduler.Model;

namespace RideScheduler.Tests
{
    public class TestDataProvider : DataProvider
    {
        Dictionary<string, Rider> Riders { get; } = new Dictionary<string, Rider>();
        Dictionary<Guid, Trip> Trips { get; } = new Dictionary<Guid, Trip>();

        protected override Task CreateRiderAsync(Rider rider)
        {
            Riders.Add(rider.Username, rider);
            return Task.CompletedTask;
        }

        protected override Task<Rider?> GetRiderAsync(string username)
        {
            if (Riders.TryGetValue(username, out var rider))
                return Task.FromResult<Rider?>(rider);

            return Task.FromResult<Rider?>(null);
        }
    }
}