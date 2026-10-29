namespace Gamification.Api.Events;

public class LevelUpEventArgs:IGameEvent
{
    public Guid PlayerId { get; }
    public DateTime OccurredAt { get; }
    public int OldLevel { get; }
    public int NewLevel { get; }

    public LevelUpEventArgs(Guid playerId,  int oldLevel, int newLevel)
    {
        PlayerId = playerId;
        OccurredAt = DateTime.UtcNow;
        OldLevel = oldLevel;
        NewLevel = newLevel;
    }
}