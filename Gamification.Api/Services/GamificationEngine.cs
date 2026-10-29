using System.Security.Cryptography;
using System.Text;
using Gamification.Api.DTOs; // Не забудь про DTO
using Gamification.Api.Events;
using Gamification.Api.Interfaces;
using Gamification.Api.Services; // Для DtoMappingService
using Gamification.Core.Models;

namespace Gamification.Api;

public class GamificationEngine
{
    private readonly IPlayerRepository _repository;
    private readonly IXpCalculator _xpCalculator;
    private readonly IAchievementEvaluator _evaluator;
    private readonly ILevelService _levelService;
    private readonly IEnumerable<AchievementDefinition> _catalog;
    private readonly DtoMappingService _mapper; // ДОБАВИЛИ

    public GamificationEngine(IPlayerRepository repository, IXpCalculator xpCalculator,
        IAchievementEvaluator evaluator, ILevelService levelService, 
        IEnumerable<AchievementDefinition> catalog, DtoMappingService mapper) // ДОБАВИЛИ mapper
    {
        _repository = repository;
        _xpCalculator = xpCalculator;
        _evaluator = evaluator;
        _levelService = levelService;
        _catalog = catalog;
        _mapper = mapper;
    }

    // --- СТАРЫЕ МЕТОДЫ (ОСТАВЛЯЕМ КАК БЫЛИ) ---

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);
        return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
    }

    private static bool VerifyPassword(string password, string stored)
    {
        var parts = stored.Split('.');
        if (parts.Length != 2) return false;
        var salt = Convert.FromBase64String(parts[0]);
        var expected = Convert.FromBase64String(parts[1]);
        var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);
        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }
    public PlayerProfile Register(string nickname, string email, string password)
    {
        if (_repository.GetAll().Any(p => p.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Email уже занят.");

        var player = new PlayerProfile(nickname, email, HashPassword(password));
        _evaluator.Evaluate(player, new LevelUpEventArgs(player.Id, 0, 1));
        _repository.Add(player);
        return player;
    }

    public PlayerProfile Login(string email, string password)
    {
        var player = _repository.GetAll()
            .FirstOrDefault(p => p.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (player == null || !VerifyPassword(password, player.PasswordHash))
            throw new UnauthorizedAccessException("Неверный email или пароль.");

        return player;
    }

    public (PlayerProfile Player, XpResult Xp, IReadOnlyCollection<PlayerAchievement> Unlocked) GrantXp(
        Guid playerId, int amount, string source)
    {
        var player = GetProfile(playerId);

        var xp = _xpCalculator.AddXp(player, amount, source);
        var unlocked = _evaluator.Evaluate(player, new XpGainedEventArgs(player.Id, amount, source));

        _repository.Save();
        return (player, xp, unlocked);
    }

    public PlayerProfile GetProfile(Guid playerId)
    {
        var player = _repository.GetById(playerId);
        if (player == null)
            throw new KeyNotFoundException("Игрок не найден");

        return player;
    }

    public IReadOnlyList<(AchievementDefinition Definition, PlayerAchievement Received)> GetPlayerAchievements(Guid playerId)
    {
        var player = GetProfile(playerId);

        return player.UnlockedAchievements
            .OrderBy(a => a.UnlockedAt)
            .Select(a => (Definition: _catalog.First(d => d.Code == a.AchievementId), Received: a))
            .ToList();
    }

    public IEnumerable<AchievementDefinition> GetCatalog() => _catalog;

    public (int Level, int Xp, int XpToNext, double Percent) GetProgress(PlayerProfile player)
    {
        int nextLevelXp = _levelService.GetXpForLevel(player.Level + 1);
        double fraction = _levelService.GetProgressToNextLevel(player.TotalXp);

        return (player.Level, player.TotalXp, nextLevelXp - player.TotalXp, Math.Round(fraction * 100, 2));
    }

    public IReadOnlyList<PlayerProfile> GetLeaderboard(int top)
    {
        return _repository.GetAll()
            .OrderByDescending(p => p.TotalXp)
            .Take(top)
            .ToList();
    }

    // --- НОВЫЕ МЕТОДЫ ДЛЯ DTO (ДОБАВЛЯЕМ В КОНЕЦ) ---

    public PlayerResponseDto GetProfileDto(Guid playerId)
    {
        var player = GetProfile(playerId);
        var progress = GetProgress(player).Percent;
        return _mapper.ToPlayerDto(player, progress);
    }

    public ProgressResponseDto GetProgressDto(Guid playerId)
    {
        var player = GetProfile(playerId);
        var p = GetProgress(player);
        return new ProgressResponseDto
        {
            CurrentLevel = p.Level,
            CurrentXp = p.Xp,
            XpToNextLevel = p.XpToNext,
            Percent = p.Percent
        };
    }

    public XpResultResponseDto GrantXpDto(Guid currentUserId, Guid targetPlayerId, int amount, string source)
    {
        if (currentUserId != targetPlayerId)
            throw new UnauthorizedAccessException("Нельзя начислять опыт другому игроку.");

        var (player, xp, unlocked) = GrantXp(targetPlayerId, amount, source);

        return new XpResultResponseDto
        {
            TotalXp = player.TotalXp,
            Level = player.Level,
            LevelUps = xp.LevelsGained,
            Unlocked = unlocked.Select(_mapper.ToAchievementDto).ToList()
        };
    }

    public List<AchievementResponseDto> GetPlayerAchievementsDto(Guid playerId)
    {
        var achievements = GetPlayerAchievements(playerId);
        return achievements.Select(x => _mapper.ToAchievementDto(x.Received)).ToList();
    }

    public List<AchievementResponseDto> GetCatalogDto()
    {
        return _catalog.Select(d => _mapper.ToAchievementDto(d)).ToList();
    }

    public List<PlayerResponseDto> GetLeaderboardDto(int top)
    {
        return GetLeaderboard(top).Select(p => 
        {
            var progress = GetProgress(p).Percent;
            return _mapper.ToPlayerDto(p, progress);
        }).ToList();
    }
}