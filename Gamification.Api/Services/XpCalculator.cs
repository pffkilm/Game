using Gamification.Api.Events;
using Gamification.Api.Interfaces;
using Gamification.Core.Models;

namespace Gamification.Api.Services;

public class XpCalculator : IXpCalculator
{
    
    private readonly ILevelService _levelService;

    public event Action<XpGainedEventArgs>? XpGained;
    public event Action<LevelUpEventArgs>? LevelUp;

    public XpCalculator(ILevelService levelService)
    {
        _levelService = levelService;
    }
    
    public XpResult AddXp(PlayerProfile player, int amount, string source)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Количество опыта должно быть положительным");
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException("Источник не может быть пустым", nameof(source));

        int oldLevel = player.Level;

        player.AddXp(amount);
        int newLevel = _levelService.GetLevelForXp(player.TotalXp);

        if (newLevel > oldLevel)
        {
            player.SetLevel(newLevel);
            LevelUp?.Invoke(new LevelUpEventArgs(player.Id, oldLevel, newLevel));
        }

        XpGained?.Invoke(new XpGainedEventArgs(player.Id, amount, source));

        var transaction = new XpTransaction(player.Id, amount, source.Trim());
        player.Transactions.Add(transaction);
        return new XpResult(transaction, oldLevel, newLevel);
    }
    
    

}