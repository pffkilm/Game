namespace Gamification.Api.Interfaces;

public interface ILevelService
{
    int GetLevelForXp(int xp);  // по опыту возвращает уровень.
    int GetXpForLevel(int level); //по уровню возвращает сколько всего опыта нужно.
    double GetProgressToNextLevel(int xp);  // по опыту возвращает долю прогресса от 0.0 до 1.0.
    
}