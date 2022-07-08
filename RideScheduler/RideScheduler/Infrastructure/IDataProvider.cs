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
        Task CreateRiderAsync(Rider user);
        Task<Rider?> GetRiderAsync(string username, string password);
    }
}
