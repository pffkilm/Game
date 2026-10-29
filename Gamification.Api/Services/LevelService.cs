using Gamification.Api.Interfaces;

namespace Gamification.Api.Services;

public class LevelService : ILevelService
{
    private const int Base = 100;
    
    public int GetLevelForXp(int xp)
    {
        int level = 1;
        while (GetXpForLevel(level + 1 )<= xp)
        {
            
            level++;

        }
        return level;
    }

    public int GetXpForLevel(int level)
    {
        return Base * level* (level -1 )/2;
    }

    public double GetProgressToNextLevel(int xp)
    {
        int currentLevel = GetLevelForXp(xp);
        int xpForCurrent = GetXpForLevel(currentLevel);
        int xpForNext = GetXpForLevel(currentLevel +1);
        
        return (double)(xp - xpForCurrent) /(xpForNext - xpForCurrent);
    }
}