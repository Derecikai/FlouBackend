using FlouBackend.Data.Entities;

namespace FlouBackend.Business.Interfaces.IRepositories;

public interface IItemRepository
{
    Task<Item> AddAsync(Item item);
    Task<Item?> GetByIdAsync(Guid id, string userId);
    Task<IEnumerable<Item>> GetAllForUserAsync(string userId);
    Task SaveChangesAsync();
}
