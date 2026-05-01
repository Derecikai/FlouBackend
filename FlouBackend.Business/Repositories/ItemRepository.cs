using FlouBackend.Business.Interfaces.IRepositories;
using FlouBackend.Data.Context;
using FlouBackend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlouBackend.Business.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _context;

    public ItemRepository(AppDbContext context) => _context = context;

    public async Task<Item> AddAsync(Item item)
    {
        await _context.Items.AddAsync(item);
        return item;
    }

    public async Task<Item?> GetByIdAsync(Guid id, string userId) =>
        await _context.Items
            .Include(i => i.ItemType)
            .Include(i => i.UrlDetail)
            .Include(i => i.CodeDetail)
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId && !i.IsDeleted);

    public async Task<IEnumerable<Item>> GetAllForUserAsync(string userId) =>
        await _context.Items
            .Include(i => i.ItemType)
            .Include(i => i.UrlDetail)
            .Include(i => i.CodeDetail)
            .Where(i => i.UserId == userId && !i.IsDeleted)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
