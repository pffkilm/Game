using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Gamification.Core.Models;

public class AchievementDefinition
{
    [Key]
    public string  Code { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int XpReward { get; private set; }
    public AchievementRuleType RuleType { get; private set; }
    public bool IsSecret { get; private set; }
    public int RuleParam { get; private set; }
    [NotMapped] public string? RuleSource { get; set; }
    [NotMapped] public int SecondaryParam { get; set; }
    
    public AchievementDefinition(string code, string title, string description, int xpReward, AchievementRuleType ruleType, int ruleParam, bool isSecret = false)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Код не может быть пустым.", nameof(code));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название не может быть пустым.", nameof(title));

        if (xpReward < 0)
            throw new ArgumentOutOfRangeException(nameof(xpReward), "Награда не может быть отрицательной.");

        Code = code;
        Title = title;
        Description = description;
        XpReward = xpReward;
        RuleType = ruleType;
        RuleParam = ruleParam;
        IsSecret = isSecret;
    }
}
