using System;
using System.Collections.Generic;

namespace FlouBackend.Data.Entities;

public partial class CodeDetail
{
    public Guid ItemId { get; set; }

    public string? Language { get; set; }

    public virtual Item Item { get; set; } = null!;
}
