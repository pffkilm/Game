namespace Gamification.Api.Events;

public class AchievementUnlockedEventArgs: IGameEvent
{
    public Guid PlayerId { get; }
    
    public DateTime OccurredAt { get; }
    public string AchievementCode { get; }

    public AchievementUnlockedEventArgs(Guid playerId, string achievementCode)
    {
        if(string.IsNullOrEmpty(achievementCode))
            throw new ArgumentException("Код достижения не может быть пустым",nameof(achievementCode));
        
        
        PlayerId = playerId;
        OccurredAt = DateTime.UtcNow;
        AchievementCode = achievementCode;
    }
}