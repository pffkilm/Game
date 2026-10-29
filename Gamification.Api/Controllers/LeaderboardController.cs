using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/leaderboard")]
public class LeaderboardController : ControllerBase
{
    private readonly GamificationEngine _engine;

    public LeaderboardController(GamificationEngine engine)
    {
        _engine = engine;
    }

    [HttpGet]
    public IActionResult GetLeaderboard([FromQuery, Range(1, 100)] int top = 10)
    {
        return Ok(new { status = true, data = _engine.GetLeaderboardDto(top) });
    }
}