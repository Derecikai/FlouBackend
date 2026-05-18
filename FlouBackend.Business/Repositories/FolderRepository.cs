using FlouBackend.Business.Interfaces.IRepositories;
using FlouBackend.Data.Context;
using FlouBackend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlouBackend.Business.Repositories;

public class FolderRepository : IFolderRepository
{
    private readonly AppDbContext _context;

    public FolderRepository(AppDbContext context) => _context = context;

    public async Task<Folder> AddAsync(Folder folder)
    {
        await _context.Folders.AddAsync(folder);
        return folder;
    }

    public async Task<Folder?> GetByIdAsync(Guid id, string userId) =>
        await _context.Folders
            .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId && !f.IsDeleted);

    public async Task<IEnumerable<Folder>> GetRootFoldersAsync(string userId) =>
        await _context.Folders
            .Where(f => f.UserId == userId && f.ParentFolderId == null && !f.IsDeleted)
            .OrderBy(f => f.Name)
            .ToListAsync();

    public async Task<IEnumerable<Folder>> GetChildFoldersAsync(Guid parentId, string userId) =>
        await _context.Folders
            .Where(f => f.UserId == userId && f.ParentFolderId == parentId && !f.IsDeleted)
            .OrderBy(f => f.Name)
            .ToListAsync();

    public async Task<IEnumerable<Item>> GetItemsInFolderAsync(Guid folderId, string userId) =>
        await _context.Items
            .Include(i => i.ItemType)
            .Include(i => i.UrlDetail)
            .Include(i => i.CodeDetail)
            .Where(i => i.UserId == userId && i.FolderId == folderId && !i.IsDeleted)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
