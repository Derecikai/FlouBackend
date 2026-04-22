using FlouBackend.Business.Interfaces.Repositories;
using FlouBackend.Data.Context;
using FlouBackend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlouBackend.Business.Repositories;

public class FolderRepository : IFolderRepository
{
    private readonly AppDbContext _context;

    public FolderRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Folder>> GetAllForUserAsync(string userId) =>
        await _context.Folders
            .Where(f => f.UserId == userId)
            .Include(f => f.Items)
            .ToListAsync();

    public async Task<Folder?> GetByIdAsync(int id, string userId) =>
        await _context.Folders
            .Include(f => f.Items)
            .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);

    public async Task AddAsync(Folder folder) => await _context.Folders.AddAsync(folder);

    public Task DeleteAsync(Folder folder)
    {
        _context.Folders.Remove(folder);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
