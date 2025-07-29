using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Session
{
    public int SessionId { get; set; }

    public int? ComputerId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual Computer? Computer { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
