using Gamification.Core.Models;
using Gamification.Api.Events;

namespace Gamification.Api.Interfaces;

public interface IAchievementEvaluator
{
    IReadOnlyCollection<PlayerAchievement> Evaluate(PlayerProfile player, IGameEvent gameEvent);
    
}