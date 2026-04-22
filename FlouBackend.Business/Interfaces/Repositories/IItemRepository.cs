using FlouBackend.Data.Entities;

namespace FlouBackend.Business.Interfaces.Repositories;

public interface IItemRepository
{
    Task<IEnumerable<Item>> GetByFolderAsync(int folderId, string userId);
    Task<Item?> GetByIdAsync(int id, string userId);
    Task AddAsync(Item item);
    Task DeleteAsync(Item item);
    Task SaveChangesAsync();
}
