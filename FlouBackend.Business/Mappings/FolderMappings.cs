using FlouBackend.Business.DTOs.Requests.FolderRequests;
using FlouBackend.Business.DTOs.Responses.FolderResponses;
using FlouBackend.Data.Entities;

namespace FlouBackend.Business.Mappings;

public static class FolderMappings
{
    // CreateFolderRequest + userId → Folder entity
    // Note: Id, CreatedAt, UpdatedAt are handled by the DB
    public static Folder ToEntity(this CreateFolderRequest request, string userId) => new()
    {
        UserId         = userId,
        Name           = request.Name,
        ParentFolderId = request.ParentFolderId
    };

    // Folder entity → FolderResponse DTO
    public static FolderResponse ToResponse(this Folder folder) => new()
    {
        Id             = folder.Id,
        Name           = folder.Name,
        ParentFolderId = folder.ParentFolderId,
        CreatedAt      = folder.CreatedAt,
        UpdatedAt      = folder.UpdatedAt
    };

    // UpdateFolderRequest → mutate existing Folder
    public static void ApplyTo(this UpdateFolderRequest request, Folder folder)
    {
        folder.Name           = request.Name;
        folder.ParentFolderId = request.ParentFolderId;
        folder.UpdatedAt      = DateTime.UtcNow;
    }
}
