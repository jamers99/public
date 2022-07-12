using Microsoft.AspNetCore.Mvc;
using RideScheduler.Infrastructure;
using RideScheduler.Model;
using System.Text.Json;

namespace RideScheduler.Controllers;

[ApiController]
[Route("[controller]")]
public class RideController : RideControllerBase
{
    private readonly ILogger<RideController> _logger;

    public RideController(ILogger<RideController> logger, DataProvider dataProvider) : base(dataProvider)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<string>> GetAsync()
    {
        if (await GetUserAsync() is not Rider rider)
            return Unauthorized();

        return rider.Username;
    }

    [HttpPost]
    public async Task PostAsync(Rider rider, string password)
    {
        await DataProvider.CreateRiderAsync(rider, password);
    }
}
