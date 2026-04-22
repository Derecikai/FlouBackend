using FlouBackend.Business.DTOs.Requests;
using FlouBackend.Business.DTOs.Responses;
using FlouBackend.Business.Interfaces.Repositories;
using FlouBackend.Business.Interfaces.Services;
using FlouBackend.Business.Mappings;

namespace FlouBackend.Business.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _repo;

    public ItemService(IItemRepository repo) => _repo = repo;

    public async Task<IEnumerable<ItemResponse>> GetByFolderAsync(int folderId, string userId)
    {
        var items = await _repo.GetByFolderAsync(folderId, userId);
        return items.Select(i => i.ToResponse());
    }

    public async Task<ItemResponse> CreateAsync(CreateItemRequest request, string userId)
    {
        var item = request.ToEntity(userId);
        await _repo.AddAsync(item);
        await _repo.SaveChangesAsync();
        return item.ToResponse();
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var item = await _repo.GetByIdAsync(id, userId);
        if (item is null) return false;

        await _repo.DeleteAsync(item);
        await _repo.SaveChangesAsync();
        return true;
    }
}
