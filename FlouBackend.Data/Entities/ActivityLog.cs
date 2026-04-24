using System;
using System.Collections.Generic;

namespace FlouBackend.Data.Entities;

public partial class ActivityLog
{
    public long Id { get; set; }

    public string UserId { get; set; } = null!;

    public int ActionTypeId { get; set; }

    public int EntityTypeId { get; set; }

    public string? EntityId { get; set; }

    public string? EntityName { get; set; }

    public string? Description { get; set; }

    public string? Metadata { get; set; }

    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ActionType ActionType { get; set; } = null!;

    public virtual EntityType EntityType { get; set; } = null!;
}
