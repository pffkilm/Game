using Gamification.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Core.Data;

public class AppDbContext : DbContext
{
    public DbSet<PlayerProfile> Players { get; set; }
    public DbSet<XpTransaction> XpTransactions { get; set; }
    public DbSet<PlayerAchievement> PlayerAchievements { get; set; }
    public DbSet<AchievementDefinition> Achievements { get; set; } 
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Составной ключ для полученных достижений
        modelBuilder.Entity<PlayerAchievement>()
            .HasKey(pa => new { pa.PlayerId, pa.AchievementId });

        // 2. Связь: Игрок -> Полученные достижения
        modelBuilder.Entity<PlayerAchievement>()
            .HasOne<PlayerProfile>()
            .WithMany(p => p.UnlockedAchievements)
            .HasForeignKey(pa => pa.PlayerId);
        modelBuilder.Entity<PlayerProfile>().Property(p => p.Id).ValueGeneratedNever();
        modelBuilder.Entity<XpTransaction>().Property(t => t.Id).ValueGeneratedNever();
      
        // 3. Связь: Игрок -> Транзакции
        modelBuilder.Entity<XpTransaction>()
            .HasOne<PlayerProfile>()
            .WithMany(p => p.Transactions)
            .HasForeignKey(t => t.PlayerId);

        // 4. Начальное заполнение базы (Seeding) - как в лекции!
        modelBuilder.Entity<AchievementDefinition>().HasData(
            new { Code = "FIRST_STEPS", Title = "Первый шаг", Description = "Игрок зарегистрирован", XpReward = 10, RuleType = AchievementRuleType.LevelReached, RuleParam = 1, IsSecret = false },
            new { Code = "LEVEL_2", Title = "Уровень 2", Description = "Достигнут 2-й уровень", XpReward = 25, RuleType = AchievementRuleType.LevelReached, RuleParam = 2, IsSecret = false },
            new { Code = "LEVEL_5", Title = "Уровень 5", Description = "Достигнут 5-й уровень", XpReward = 100, RuleType = AchievementRuleType.LevelReached, RuleParam = 5, IsSecret = false },
            new { Code = "LEVEL_10", Title = "Уровень 10", Description = "Достигнут 10-й уровень", XpReward = 300, RuleType = AchievementRuleType.LevelReached, RuleParam = 10, IsSecret = false },
            new { Code = "COLLECTOR_500", Title = "Коллекционер 500", Description = "Набрано 500 XP", XpReward = 50, RuleType = AchievementRuleType.TotalXpReached, RuleParam = 500, IsSecret = false },
            new { Code = "COLLECTOR_5000", Title = "Коллекционер 5000", Description = "Набрано 5000 XP", XpReward = 250, RuleType = AchievementRuleType.TotalXpReached, RuleParam = 5000, IsSecret = false },
            new { Code = "MARATHON_7", Title = "Марафонец", Description = "7 действий с любым источником", XpReward = 40, RuleType = AchievementRuleType.ActionCountReached, RuleParam = 7, IsSecret = false },
            new { Code = "DEDICATED", Title = "Преданный", Description = "10 действий с источником lesson", XpReward = 60, RuleType = AchievementRuleType.ActionCountReached, RuleParam = 10, IsSecret = false },
            new { Code = "SHOPAHOLIC", Title = "Шопоголик", Description = "5 действий с источником purchase", XpReward = 50, RuleType = AchievementRuleType.ActionCountReached, RuleParam = 5, IsSecret = false },
            new { Code = "SECRET", Title = "Секретное", Description = "Секретное достижение", XpReward = 200, RuleType = AchievementRuleType.LevelReached, RuleParam = 7, IsSecret = true }
        );
    }
}
