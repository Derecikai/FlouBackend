namespace FlouBackend.Business.DTOs.Requests.FolderRequests;

public class UpdateFolderRequest
{
    public string Name { get; set; } = null!;
    public Guid? ParentFolderId { get; set; }   // null = move to root
}
