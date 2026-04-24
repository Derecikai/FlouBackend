using System;
using System.Collections.Generic;

namespace FlouBackend.Data.Entities;

public partial class EntityType
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
}
