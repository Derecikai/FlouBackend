namespace FlouBackend.Business.DTOs.Requests;

public class CreateItemRequest
{
    // Required for all item types
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int ItemTypeId { get; set; }
    public Guid? FolderId { get; set; }

    // Only used when ItemTypeId = url (2)
    public string? Url { get; set; }
    public string? Domain { get; set; }
    public string? ThumbnailUrl { get; set; }

    // Only used when ItemTypeId = code (3)
    public string? Language { get; set; }
}
