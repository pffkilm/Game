namespace Gamification.Api.Events;

public class XpGainedEventArgs : IGameEvent
{
    public Guid PlayerId { get; }
    
    public DateTime OccurredAt { get; }
    
    public int Amount { get; }
    
    public string Source { get; }

    public XpGainedEventArgs(Guid playerId,  int amount, string source)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException("Количество опыта должно быть положительным",nameof(amount));
        
        if(string.IsNullOrWhiteSpace(source))
            throw new ArgumentException( "Источник не может быть пустым", nameof(source));
        
        PlayerId = playerId;
        OccurredAt = DateTime.UtcNow;
        Amount = amount;
        Source = source.Trim();
    }
}