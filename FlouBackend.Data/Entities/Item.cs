using Microsoft.AspNetCore.Identity;

namespace FlouBackend.Data.Entities;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public int FolderId { get; set; }
    public Folder? Folder { get; set; }

    public string UserId { get; set; } = null!;
    public IdentityUser? User { get; set; }
}
