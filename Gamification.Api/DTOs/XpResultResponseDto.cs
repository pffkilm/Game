namespace Gamification.Api.DTOs;


public class XpResultResponseDto
{
    public int TotalXp { get; set; }
    public int Level { get; set; }
    public int LevelUps { get; set; }
    public List<AchievementResponseDto> Unlocked { get; set; } = new();
}