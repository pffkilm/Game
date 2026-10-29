using System.ComponentModel.DataAnnotations;
namespace Gamification.Core.Models;

public class PlayerProfile
{
    [Key]
    public Guid Id { get; private set; }
    public string Nickname { get; private set; }
    public int TotalXp { get; private set;}
    public int Level { get; private set; }
    public ICollection<PlayerAchievement> UnlockedAchievements { get; private set; }
    public ICollection<XpTransaction> Transactions { get; private set; } 
    public DateTime CreatedAt { get; private set; }

// Внутри класса PlayerProfile добавь свойства:
    [Required] // не забудь using System.ComponentModel.DataAnnotations;
    [EmailAddress]
    public string Email { get; private set; }

    [Required]
    public string PasswordHash { get; private set; }

// Измени конструктор:
    public PlayerProfile(string nickname, string email, string passwordHash) // добавили 2 параметра
    {
        if (string.IsNullOrWhiteSpace(nickname))
            throw new ArgumentException("Никнейм не может быть пустым", nameof(nickname));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email не может быть пустым", nameof(email));
    
        Id = Guid.NewGuid();
        Nickname = nickname.Trim();
        Email = email.Trim(); // сохраняем email
        PasswordHash = passwordHash; // сохраняем хеш пароля
        TotalXp = 0;
        Level = 1;
        UnlockedAchievements = new List<PlayerAchievement>();
        CreatedAt = DateTime.UtcNow;
        Transactions = new List<XpTransaction>();
    }

    public void AddXp(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Опыт должен быть положительным", nameof(amount));

        TotalXp += amount;
    }

    public void UnlockAchievement(PlayerAchievement achievement)
    {
        if (achievement == null)
            throw new ArgumentNullException(nameof(achievement));
    

        if (UnlockedAchievements.Any(a => a.AchievementId == achievement.AchievementId))
            return;
         
        UnlockedAchievements.Add(achievement);

    }


    public void SetLevel(int newLevel)
    {
        if (newLevel < Level)
            throw new InvalidOperationException("Уровень не может уменьшаться.");

        Level = newLevel;

    }
}