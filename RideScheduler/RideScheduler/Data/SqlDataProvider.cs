using RideScheduler.Infrastructure;
using RideScheduler.Model;
using System.Text.Json;

namespace RideScheduler.Data
{
    public class SqlDataProvider : IDataProvider
    {
        public async Task CreateUserAsync(User user)
        {
            await File.AppendAllLinesAsync("users.txt", new[] { JsonSerializer.Serialize(user) });
        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            var lines = await File.ReadAllLinesAsync("users.txt");
            foreach (var line in lines)
            {
                var user = JsonSerializer.Deserialize<User>(line);
                if (user?.Name == username)
                    return user;
            }

            return null;
        }
    }
}
