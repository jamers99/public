using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Cosmos.Linq;
using RideScheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideScheduler.Infrastructure
{
    public abstract class DataProvider
    {
        public async Task CreateRiderAsync(Rider rider, string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            rider.HashedPassword = new PasswordHasher<object?>().HashPassword(null, password);
            await CreateRiderAsync(rider);
        }

        protected abstract Task CreateRiderAsync(Rider rider);

        public async Task<Rider?> GetRiderAsync(string username, string password)
        {
            var rider = await GetRiderAsync(username);
            if (rider == null)
                return null;

            if (new PasswordHasher<object?>().VerifyHashedPassword(null, rider.HashedPassword, password) == PasswordVerificationResult.Failed)
                return null;

            return rider;
        }

        protected abstract Task<Rider?> GetRiderAsync(string username);
    }
}
