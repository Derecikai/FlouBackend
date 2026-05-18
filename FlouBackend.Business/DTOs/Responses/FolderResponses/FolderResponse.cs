namespace FlouBackend.Business.DTOs.Responses.FolderResponses;

public class FolderResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid? ParentFolderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
