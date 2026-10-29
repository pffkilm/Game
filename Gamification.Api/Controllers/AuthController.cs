using Gamification.Api.DTOs;
using Gamification.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly GamificationEngine _engine;
    private readonly JwtTokenService _jwtService;

    public AuthController(GamificationEngine engine, JwtTokenService jwtService)
    {
        _engine = engine;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequestDto dto)
    {
        try
        {
            var player = _engine.Register(dto.Nickname, dto.Email, dto.Password);
            return Ok(new
            {
                status = true,
                message = "Успешно зарегистрирован.",
                data = new { id = player.Id, nickname = player.Nickname, email = player.Email }
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { status = false, message = ex.Message });
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] AuthRequestDto dto)
    {
        try
        {
            var player = _engine.Login(dto.Email, dto.Password);
            var token = _jwtService.GenerateToken(player);
            return Ok(new { status = true, data = new AuthResponseDto { Token = token, ExpiresAt = DateTime.UtcNow.AddMinutes(60) } });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { status = false, message = ex.Message });
        }
    }

    [HttpGet("/api/test/throw")]
    public IActionResult Throw() => throw new Exception("Тестовая ошибка 500");
}