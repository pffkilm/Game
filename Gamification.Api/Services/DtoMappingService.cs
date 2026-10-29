using Gamification.Api.DTOs;
using Gamification.Core.Models;

namespace Gamification.Api.Services;

public class DtoMappingService
{
    private readonly IEnumerable<AchievementDefinition> _catalog;

    public DtoMappingService(IEnumerable<AchievementDefinition> catalog)
    {
        _catalog = catalog;
    }

    public PlayerResponseDto ToPlayerDto(PlayerProfile p, double progressPercent)
    {
        return new PlayerResponseDto
        {
            Id = p.Id,
            Nickname = p.Nickname,
            Level = p.Level,
            TotalXp = p.TotalXp,
            ProgressPercent = progressPercent,
            CreatedAt = p.CreatedAt
        };
    }

    public AchievementResponseDto ToAchievementDto(PlayerAchievement a)
    {
        var def = _catalog.FirstOrDefault(d => d.Code == a.AchievementId);
        return new AchievementResponseDto
        {
            Code = a.AchievementId,
            Title = def?.Title ?? string.Empty,
            Description = def?.Description ?? string.Empty,
            XpReward = def?.XpReward ?? 0,
            IsSecret = def?.IsSecret ?? false,
            UnlockedAt = a.UnlockedAt
        };
    }

    public AchievementResponseDto ToAchievementDto(AchievementDefinition d, DateTime? unlockedAt = null)
    {
        return new AchievementResponseDto
        {
            Code = d.Code,
            Title = d.Title,
            Description = d.Description,
            XpReward = d.XpReward,
            IsSecret = d.IsSecret,
            UnlockedAt = unlockedAt
        };
    }
}