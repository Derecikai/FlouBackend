using System;
using System.Collections.Generic;

namespace FlouBackend.Data.Entities;

public partial class Item
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = null!;

    public Guid? FolderId { get; set; }

    public int ItemTypeId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsArchived { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual CodeDetail? CodeDetail { get; set; }

    public virtual Folder? Folder { get; set; }

    public virtual ICollection<ItemTag> ItemTags { get; set; } = new List<ItemTag>();

    public virtual ItemType ItemType { get; set; } = null!;

    public virtual UrlDetail? UrlDetail { get; set; }
}
