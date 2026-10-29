using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

[Authorize]
[ApiController]
[Route("api")]
public class AchievementsController : ControllerBase
{
    private readonly GamificationEngine _engine;

    public AchievementsController(GamificationEngine engine)
    {
        _engine = engine;
    }

    [HttpGet("achievements")]
    public IActionResult GetCatalog()
    {
        return Ok(new { status = true, data = _engine.GetCatalogDto() });
    }
    [HttpGet("players/{id}/achievements")]
    public IActionResult GetPlayerAchievements(Guid id)
    {
        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (id != currentUserId)
            return StatusCode(403, new { status = false, message = "Доступ к чужому профилю запрещён." });

        try { return Ok(new { status = true, data = _engine.GetPlayerAchievementsDto(id) }); }
        catch (KeyNotFoundException) { return NotFound(new { status = false, message = "Игрок не найден." }); }
    }
}