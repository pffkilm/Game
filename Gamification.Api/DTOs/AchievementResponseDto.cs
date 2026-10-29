namespace Gamification.Api.DTOs;


public class AchievementResponseDto
{
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int XpReward { get; set; }
    public bool IsSecret { get; set; }
    public DateTime? UnlockedAt { get; set; }
}