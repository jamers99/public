using Microsoft.AspNetCore.Mvc;
using RideScheduler.Model;
using System.Text;

namespace RideScheduler.Controllers
{
    public class RideControllerBase : ControllerBase
    {
        public async Task<User?> GetUserAsync()
        {
            if (Request.Headers.TryGetValue("Authorization", out var auth))
            {
                var base64 = auth.ToString().Split(' ')[1];
                var token = Encoding.UTF8.GetString(Convert.FromBase64String(base64)).Split(':');
                var username = token[0];
                var password = token[1];
                return new User() { Name = username };
            }

            return null;
        }
    }
}
