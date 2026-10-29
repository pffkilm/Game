namespace Gamification.Api.DTOs;


public class ProgressResponseDto
{
    public int CurrentLevel { get; set; }
    public int CurrentXp { get; set; }
    public int XpToNextLevel { get; set; }
    public double Percent { get; set; }
}