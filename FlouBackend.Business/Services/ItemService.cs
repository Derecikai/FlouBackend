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

    public async Task<ItemResponse> CreateAsync(CreateItemRequest request, string userId)
    {
        // Step 1 — convert the request DTO into an Item entity
        var item = request.ToEntity(userId);

        // Step 2 — attach type-specific detail if applicable

        if (request.ItemTypeId == 2 && request.Url is not null)
        {
            item.UrlDetail = new UrlDetail
            {
                Url          = request.Url,
                Domain       = request.Domain,
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

    public async Task<ItemResponse?> GetByIdAsync(Guid id, string userId)
    {
        var item = await _repo.GetByIdAsync(id, userId);
        return item?.ToResponse();
    }

    public async Task<IEnumerable<ItemResponse>> GetAllForUserAsync(string userId)
    {
        var items = await _repo.GetAllForUserAsync(userId);
        return items.Select(i => i.ToResponse());
    }
}
