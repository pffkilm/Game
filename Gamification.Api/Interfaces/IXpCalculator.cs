using Gamification.Api.Events;
using Gamification.Core.Models;

namespace Gamification.Api.Interfaces;

public interface IXpCalculator
{
    event Action<XpGainedEventArgs>? XpGained;
    event Action<LevelUpEventArgs>? LevelUp;

    XpResult AddXp(PlayerProfile player, int amount, string source);
}