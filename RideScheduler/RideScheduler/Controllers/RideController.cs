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

    public RideController(ILogger<RideController> logger, IDataProvider dataProvider) : base(dataProvider)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<string>> GetAsync()
    {
        if (await GetUserAsync() is not User user)
            return Unauthorized();

        return user.Name;
    }

    [HttpPost]
    public async Task PostAsync(User user)
    {
        await DataProvider.CreateUserAsync(user);
    }
}
