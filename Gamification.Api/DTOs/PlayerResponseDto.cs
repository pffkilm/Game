namespace Gamification.Api.DTOs;


public class PlayerResponseDto
{
    public Guid Id { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public int Level { get; set; }
    public int TotalXp { get; set; }
    public double ProgressPercent { get; set; }
    public DateTime CreatedAt { get; set; }
}