using System;
using System.Collections.Generic;

namespace FlouBackend.Data.Entities;

public partial class ItemType
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? IconName { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
