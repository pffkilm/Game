using System.ComponentModel.DataAnnotations;
namespace Gamification.Core.Models;

public class PlayerAchievement
{
    public Guid PlayerId { get; private set; }
    public string AchievementId { get; private set; }
    public DateTime UnlockedAt { get; private set; }
    public int Progress { get; private set; }


    public PlayerAchievement(Guid playerId, string achievementId ,  int progress =100)
    {
        if (string.IsNullOrWhiteSpace(achievementId))
            throw new ArgumentException("Код достижения не может быть пустым.", nameof(achievementId));

        if (progress < 0 || progress > 100)
            throw new ArgumentOutOfRangeException(nameof(progress), "Прогресс должен быть от 0 до 100");
        
        PlayerId = playerId;
        AchievementId = achievementId;
        UnlockedAt = DateTime.UtcNow;
        Progress = progress;
        
        
    }
    

}