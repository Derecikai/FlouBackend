using System;
using System.Collections.Generic;

namespace FlouBackend.Data.Entities;

public partial class Folder
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = null!;

    public Guid? ParentFolderId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Folder> InverseParentFolder { get; set; } = new List<Folder>();

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual Folder? ParentFolder { get; set; }
}
