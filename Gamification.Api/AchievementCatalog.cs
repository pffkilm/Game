using Gamification.Core.Models;

namespace Gamification.Api;

public static class AchievementCatalog
{
    public static List<AchievementDefinition> GetAchievements()
    {
        return new List<AchievementDefinition>
        {
            new("FIRST_STEPS", "Первый шаг", "Игрок зарегистрирован", 10, AchievementRuleType.LevelReached, 1),
            new("LEVEL_2", "Уровень 2", "Достигнут 2-й уровень", 25, AchievementRuleType.LevelReached, 2),
            new("LEVEL_5", "Уровень 5", "Достигнут 5-й уровень", 100, AchievementRuleType.LevelReached, 5),
            new("LEVEL_10", "Уровень 10", "Достигнут 10-й уровень", 300, AchievementRuleType.LevelReached, 10),
            new("COLLECTOR_500", "Коллекционер 500", "Набрано 500 XP", 50, AchievementRuleType.TotalXpReached, 500),
            new("COLLECTOR_5000", "Коллекционер 5000", "Набрано 5000 XP", 250, AchievementRuleType.TotalXpReached,
                5000),
            new("MARATHON_7", "Марафонец", "7 действий с любым источником", 40, AchievementRuleType.ActionCountReached,
                7),
            new("DEDICATED", "Преданный", "10 действий с источником lesson", 60, AchievementRuleType.ActionCountReached, 10) { RuleSource = "lesson" },
            new("SHOPAHOLIC", "Шопоголик", "5 действий с источником purchase", 50, AchievementRuleType.ActionCountReached, 5) { RuleSource = "purchase" },
            new("SECRET", "Секретное", "Секретное достижение", 200, AchievementRuleType.LevelReached, 7, true) { SecondaryParam = 3 }};
    }
}