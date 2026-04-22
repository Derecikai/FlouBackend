using Microsoft.AspNetCore.Identity;

namespace FlouBackend.Data.Entities;

public class Folder
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public string UserId { get; set; } = null!;
    public IdentityUser? User { get; set; }

    public ICollection<Item> Items { get; set; } = new List<Item>();
}
