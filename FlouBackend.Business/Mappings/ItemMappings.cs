using FlouBackend.Business.DTOs.Requests;
using FlouBackend.Business.DTOs.Responses;
using FlouBackend.Data.Entities;

namespace FlouBackend.Business.Mappings;

public static class ItemMappings
{
    public static ItemResponse ToResponse(this Item item) => new()
    {
        Id = item.Id,
        Name = item.Name,
        CreatedAt = item.CreatedAt,
        FolderId = item.FolderId
    };

    public static Item ToEntity(this CreateItemRequest request, string userId) => new()
    {
        Name = request.Name,
        FolderId = request.FolderId,
        UserId = userId,
        CreatedAt = DateTime.UtcNow
    };
}
