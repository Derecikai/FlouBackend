using FlouBackend.Business.DTOs.Requests;
using FlouBackend.Business.DTOs.Responses;
using FlouBackend.Data.Entities;

namespace FlouBackend.Business.Mappings;

public static class FolderMappings
{
    public static FolderResponse ToResponse(this Folder folder) => new()
    {
        Id = folder.Id,
        Name = folder.Name,
        CreatedAt = folder.CreatedAt,
        ItemCount = folder.Items?.Count ?? 0
    };

    public static Folder ToEntity(this CreateFolderRequest request, string userId) => new()
    {
        Name = request.Name,
        UserId = userId,
        CreatedAt = DateTime.UtcNow
    };
}
