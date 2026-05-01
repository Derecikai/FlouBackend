namespace FlouBackend.Business.DTOs.Responses;

public class ItemResponse
{
    // Core — always present
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int ItemTypeId { get; set; }
    public string ItemTypeCode { get; set; } = null!;  // "note", "url", "code"
    public Guid? FolderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsArchived { get; set; }

    // Only populated when ItemTypeId = url (2)
    public string? Url { get; set; }
    public string? Domain { get; set; }
    public string? ThumbnailUrl { get; set; }

    // Only populated when ItemTypeId = code (3)
    public string? Language { get; set; }
}
