namespace FlouBackend.Business.DTOs.Requests.FolderRequests;

public class CreateFolderRequest
{
    public string Name { get; set; } = null!;
    public Guid? ParentFolderId { get; set; }   // null = root level
}
