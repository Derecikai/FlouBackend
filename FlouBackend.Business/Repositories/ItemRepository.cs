using FlouBackend.Business.Interfaces.Repositories;
using FlouBackend.Data.Context;
using FlouBackend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlouBackend.Business.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _context;

    public ItemRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Item>> GetByFolderAsync(int folderId, string userId) =>
        await _context.Items
            .Where(i => i.FolderId == folderId && i.UserId == userId)
            .ToListAsync();

    public async Task<Item?> GetByIdAsync(int id, string userId) =>
        await _context.Items.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

    public async Task AddAsync(Item item) => await _context.Items.AddAsync(item);

    public Task DeleteAsync(Item item)
    {
        _context.Items.Remove(item);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
