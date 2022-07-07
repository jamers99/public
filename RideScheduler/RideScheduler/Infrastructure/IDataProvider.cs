using RideScheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideScheduler.Infrastructure
{
    public interface IDataProvider
    {
        Task<User?> GetUserAsync(string username, string password);
        Task CreateUserAsync(User user);
    }
}
