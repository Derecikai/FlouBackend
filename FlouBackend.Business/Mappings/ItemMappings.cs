using FlouBackend.Business.DTOs.Requests;
using FlouBackend.Business.DTOs.Responses;
using FlouBackend.Data.Entities;

namespace FlouBackend.Business.Mappings;

public static class ItemMappings
{
    // CreateItemRequest + userId → Item entity
    // Note: Id, CreatedAt, UpdatedAt are handled by the DB (newsequentialid, sysutcdatetime)
    public static Item ToEntity(this CreateItemRequest request, string userId) => new()
    {
        UserId    = userId,
        FolderId  = request.FolderId,
        ItemTypeId = request.ItemTypeId,
        Title     = request.Title,
        Content   = request.Content
    };

    // Item entity → ItemResponse DTO
    // Navigation properties (ItemType, UrlDetail, CodeDetail) must be loaded by the repository
    public static ItemResponse ToResponse(this Item item) => new()
    {
        Id           = item.Id,
        Title        = item.Title,
        Content      = item.Content,
        ItemTypeId   = item.ItemTypeId,
        ItemTypeCode = item.ItemType?.Code ?? string.Empty,
        FolderId     = item.FolderId,
        CreatedAt    = item.CreatedAt,
        UpdatedAt    = item.UpdatedAt,
        IsArchived   = item.IsArchived,

        // URL fields — only populated when UrlDetail exists
        Url          = item.UrlDetail?.Url,
        Domain       = item.UrlDetail?.Domain,
        ThumbnailUrl = item.UrlDetail?.ThumbnailUrl,

        // Code fields — only populated when CodeDetail exists
        Language     = item.CodeDetail?.Language
    };
}
