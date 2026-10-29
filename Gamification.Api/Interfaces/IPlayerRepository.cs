using Gamification.Core.Models;


namespace Gamification.Api.Interfaces;

public interface IPlayerRepository
{
    void Add(PlayerProfile player); // Добавляет нового игрока в хранилище.
    
    PlayerProfile? GetById(Guid id); // Возвращает игрока по идентификатору
    
    IReadOnlyList<PlayerProfile> GetAll();   // Возвращает всех игроков. Используется для построения рейтинга.
    void Save();
}