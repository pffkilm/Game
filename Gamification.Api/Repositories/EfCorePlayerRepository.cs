using Gamification.Api.Interfaces;
using Gamification.Core.Data; 
using Gamification.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Api.Repositories;

public class EfCorePlayerRepository : IPlayerRepository
{
    private readonly AppDbContext _context;
    
    public EfCorePlayerRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(PlayerProfile player)
    {
        _context.Players.Add(player);
        _context.SaveChanges();
    }

    public PlayerProfile? GetById(Guid id)
    {
        return _context.Players
            .Include(p => p.UnlockedAchievements)
            .Include(p => p.Transactions)
            .FirstOrDefault(p => p.Id == id);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public IReadOnlyList<PlayerProfile> GetAll()
    {
        return _context.Players.Include(p => p.UnlockedAchievements).ToList();
    }
}