using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gamification.Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace Gamification.Api.Services;

public class JwtTokenService
{
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(PlayerProfile player)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, player.Id.ToString()),
            new Claim(ClaimTypes.Name, player.Nickname),
            new Claim(ClaimTypes.Email, player.Email),
            new Claim(ClaimTypes.Role, "player") // Все игроки - player
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}