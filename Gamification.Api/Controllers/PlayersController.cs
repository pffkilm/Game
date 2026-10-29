using System.Security.Claims;
using Gamification.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/players")]
public class PlayersController : ControllerBase
{
    private readonly GamificationEngine _engine;

    public PlayersController(GamificationEngine engine)
    {
        _engine = engine;
    }

    private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private IActionResult Forbidden() =>
        StatusCode(403, new { status = false, message = "Доступ к чужому профилю запрещён." });

    [HttpGet("{id}")]
    public IActionResult GetProfile(Guid id)
    {
        if (id != CurrentUserId) return Forbidden();
        try { return Ok(new { status = true, data = _engine.GetProfileDto(id) }); }
        catch (KeyNotFoundException) { return NotFound(new { status = false, message = "Игрок не найден." }); }
    }

    [HttpPost("{id}/xp")]
    public IActionResult AddXp(Guid id, [FromBody] AddXpDto dto)
    {
        try
        {
            var result = _engine.GrantXpDto(CurrentUserId, id, dto.Amount, dto.Source);
            return Ok(new { status = true, data = result });
        }
        catch (UnauthorizedAccessException ex) { return StatusCode(403, new { status = false, message = ex.Message }); }
        catch (KeyNotFoundException) { return NotFound(new { status = false, message = "Игрок не найден." }); }
    }

    [HttpGet("{id}/progress")]
    public IActionResult GetProgress(Guid id)
    {
        if (id != CurrentUserId) return Forbidden();
        try { return Ok(new { status = true, data = _engine.GetProgressDto(id) }); }
        catch (KeyNotFoundException) { return NotFound(new { status = false, message = "Игрок не найден." }); }
    }
}