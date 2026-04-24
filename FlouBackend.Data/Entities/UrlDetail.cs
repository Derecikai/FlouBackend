using System;
using System.Collections.Generic;

namespace FlouBackend.Data.Entities;

public partial class UrlDetail
{
    public Guid ItemId { get; set; }

    public string Url { get; set; } = null!;

    public string? Domain { get; set; }

    public string? ThumbnailUrl { get; set; }

    public virtual Item Item { get; set; } = null!;
}
