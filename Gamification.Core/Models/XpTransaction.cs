using System.ComponentModel.DataAnnotations;
namespace Gamification.Core.Models;

public class XpTransaction
{
    [Key] 
    public Guid Id { get; private set;}
    public Guid PlayerId { get; private set; }
    public int Amount { get; private set; }
    public string Source { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public XpTransaction(Guid playerId,int amount , string source)
    {

        if (amount <= 0)
            throw new ArgumentException("Колтичество опыта должно быть положительным", nameof(amount));

        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException("Источник опыта не может быть пустым", nameof(source));
        
        Id = Guid.NewGuid();
        PlayerId = playerId;
        Amount = amount;
        Source = source;
        CreatedAt = DateTime.UtcNow;

    }
    
    
}