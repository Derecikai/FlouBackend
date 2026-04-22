using FlouBackend.Data.Entities;

namespace FlouBackend.Business.Interfaces.Repositories;

public interface IFolderRepository
{
    Task<IEnumerable<Folder>> GetAllForUserAsync(string userId);
    Task<Folder?> GetByIdAsync(int id, string userId);
    Task AddAsync(Folder folder);
    Task DeleteAsync(Folder folder);
    Task SaveChangesAsync();
}
