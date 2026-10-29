using Gamification.Api.Events;
using Gamification.Api.Interfaces;
using Gamification.Core.Models;

namespace Gamification.Api.Services;

public class AchievementEvaluator : IAchievementEvaluator
{
    private readonly IEnumerable<AchievementDefinition> _catalog;

    public AchievementEvaluator(IEnumerable<AchievementDefinition> catalog)
    {
        _catalog = catalog;
    }

    public IReadOnlyCollection<PlayerAchievement> Evaluate(PlayerProfile player, IGameEvent gameEvent)
    {
        var newAchievements = new List<PlayerAchievement>();

        foreach (var def in _catalog)
        {
            if (player.UnlockedAchievements.Any(a => a.AchievementId == def.Code))
                continue;

            bool conditionMet = def.RuleType switch
            {
                AchievementRuleType.TotalXpReached => player.TotalXp >= def.RuleParam,
                AchievementRuleType.ActionCountReached => CountActions(player, def.RuleSource) >= def.RuleParam,
                AchievementRuleType.LevelReached => player.Level >= def.RuleParam && CountSources(player) >= def.SecondaryParam,
                _ => false
            };

            if (!conditionMet)
                continue;

            var achievement = new PlayerAchievement(player.Id, def.Code);
            player.UnlockAchievement(achievement);
            newAchievements.Add(achievement);
        }

        return newAchievements;
    }

    private static int CountActions(PlayerProfile player, string? source)
    {
        return string.IsNullOrWhiteSpace(source)
            ? player.Transactions.Count
            : player.Transactions.Count(t => string.Equals(t.Source, source, StringComparison.OrdinalIgnoreCase));
    }

    private static int CountSources(PlayerProfile player)
    {
        return player.Transactions.Select(t => t.Source.ToLowerInvariant()).Distinct().Count();
    }
}