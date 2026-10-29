namespace Gamification.Core.Models;

public class XpResult
{
    
    public XpTransaction Transaction { get; }
    public int OldLevel { get; }
    public int NewLevel { get; }
    public int LevelsGained => NewLevel - OldLevel;


    public XpResult(XpTransaction transaction, int oldLevel, int newLevel)
    {
        Transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        
        OldLevel = oldLevel;
        NewLevel = newLevel;

    }
    
  
}