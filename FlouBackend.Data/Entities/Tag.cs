using System;
using System.Collections.Generic;

namespace FlouBackend.Data.Entities;

public partial class Tag
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<ItemTag> ItemTags { get; set; } = new List<ItemTag>();
}
