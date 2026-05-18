using FlouBackend.Data.Entities;

namespace FlouBackend.Business.Interfaces.IRepositories;

public interface IFolderRepository
{
    Task<Folder> AddAsync(Folder folder);
    Task<Folder?> GetByIdAsync(Guid id, string userId);
    Task<IEnumerable<Folder>> GetRootFoldersAsync(string userId);
    Task<IEnumerable<Folder>> GetChildFoldersAsync(Guid parentId, string userId);
    Task<IEnumerable<Item>> GetItemsInFolderAsync(Guid folderId, string userId);
    Task SaveChangesAsync();
}
