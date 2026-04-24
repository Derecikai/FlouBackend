using System;
using System.Collections.Generic;

namespace FlouBackend.Data.Entities;

public partial class ItemTag
{
    public Guid ItemId { get; set; }

    public int TagId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
