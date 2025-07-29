using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Computer
{
    public int ComputerId { get; set; }

    public string Name { get; set; } = null!;

    public string? Tier { get; set; }

    public decimal HourlyRate { get; set; }

    public byte? Status { get; set; }

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
