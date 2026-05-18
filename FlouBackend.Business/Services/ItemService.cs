using FlouBackend.Business.DTOs.Requests;
using FlouBackend.Business.DTOs.Responses;
using FlouBackend.Business.Interfaces.IRepositories;
using FlouBackend.Business.Interfaces.IServices;
using FlouBackend.Business.Mappings;
using FlouBackend.Data.Entities;

namespace FlouBackend.Business.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _repo;

    public ItemService(IItemRepository repo) => _repo = repo;

     //CREATE AN ITEM
    public async Task<ItemResponse> CreateAsync(CreateItemRequest request, string userId)
    {
        // Step 1 — convert the request DTO into an Item entity
        var item = request.ToEntity(userId);

        // Step 2 — attach type-specific detail if applicable

        if (request.ItemTypeId == 2 && request.Url is not null)
        {
            item.UrlDetail = new UrlDetail
            {
                Url = request.Url,
                Domain = request.Domain,
                ThumbnailUrl = request.ThumbnailUrl
            };
        }
        else if (request.ItemTypeId == 3)
        {
            item.CodeDetail = new CodeDetail
            {
                Language = request.Language
            };
        }

        // Step 3 — save to DB
        await _repo.AddAsync(item);
        await _repo.SaveChangesAsync();

        // Step 4 — reload with navigation properties so the mapping has everything it needs
        var saved = await _repo.GetByIdAsync(item.Id, userId);
        return saved!.ToResponse();
    }

    // GET AN ITEM BY ID
    public async Task<ItemResponse?> GetByIdAsync(Guid id, string userId)
    {
        var item = await _repo.GetByIdAsync(id, userId);
        return item?.ToResponse();
    }

    // GET ROOT ITEMS
    public async Task<IEnumerable<ItemResponse>> GetRootItemsAsync(string userId)
    {
        var items = await _repo.GetRootItemsAsync(userId);
        return items.Select(i => i.ToResponse());
    }

    public async Task<IEnumerable<ItemResponse>> GetAllForUserAsync(string userId)
    {
        var items = await _repo.GetAllForUserAsync(userId);
        return items.Select(i => i.ToResponse());
    }
    // DELETE AN ITEM(WE ARE SOFT DELETING)
    public async Task<bool> DeleteAsync(Guid id, string userId)
    {
        // Step 1 — find the item (already checks it belongs to this user)
        var item = await _repo.GetByIdAsync(id, userId);

        // Step 2 — if not found, tell the caller it didn't exist
        if (item is null) return false;

        // Step 3 — soft delete: mark as deleted, don't remove the row
        item.IsDeleted = true;
        item.DeletedAt = DateTime.UtcNow;

        // Step 4 — save the change to DB
        await _repo.SaveChangesAsync();

        return true;
    }

    // UPDATE AN ITEM
    public async Task<ItemResponse?> UpdateAsync(UpdateItemRequest request, Guid id, string userId)
    {
        
            var item = await _repo.GetByIdAsync(id, userId);   // tracked entity, with details loaded
            if (item is null) return null;

            request.ApplyTo(item);                              // mutate fields
            await _repo.SaveChangesAsync();                     // EF emits UPDATE

            return item.ToResponse();
        
    }
}
