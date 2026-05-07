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
        UserId = userId,
        FolderId = request.FolderId,
        ItemTypeId = request.ItemTypeId,
        Title = request.Title,
        Content = request.Content
    };

    // Item entity → ItemResponse DTO
    // Navigation properties (ItemType, UrlDetail, CodeDetail) must be loaded by the repository
    public static ItemResponse ToResponse(this Item item) => new()
    {
        Id = item.Id,
        Title = item.Title,
        Content = item.Content,
        ItemTypeId = item.ItemTypeId,
        ItemTypeCode = item.ItemType?.Code ?? string.Empty,
        FolderId = item.FolderId,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
        IsArchived = item.IsArchived,

        // URL fields — only populated when UrlDetail exists
        Url = item.UrlDetail?.Url,
        Domain = item.UrlDetail?.Domain,
        ThumbnailUrl = item.UrlDetail?.ThumbnailUrl,

        // Code fields — only populated when CodeDetail exists
        Language = item.CodeDetail?.Language
    };

    public static void ApplyTo(this UpdateItemRequest request, Item item)
    {
        item.Title = request.Title;
        item.Content = request.Content;
        item.FolderId = request.FolderId;
        item.UpdatedAt = DateTime.UtcNow;

        if (item.ItemTypeId == 2 && item.UrlDetail is not null)
        {
            item.UrlDetail.Url = request.Url!;
            item.UrlDetail.Domain = request.Domain;
            item.UrlDetail.ThumbnailUrl = request.ThumbnailUrl;
        }
        else if (item.ItemTypeId == 3 && item.CodeDetail is not null)
        {
            item.CodeDetail.Language = request.Language;
        }
    }
}
