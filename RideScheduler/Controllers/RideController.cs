using Microsoft.AspNetCore.Mvc;
using RideScheduler.Model;

namespace RideScheduler.Controllers;

[ApiController]
[Route("[controller]")]
public class RideController : RideControllerBase
{
    private readonly ILogger<RideController> _logger;

    public RideController(ILogger<RideController> logger)
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
}
