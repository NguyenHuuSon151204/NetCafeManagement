using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? CustomerId { get; set; }

    public decimal Amount { get; set; }

    public string? Type { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Customer? Customer { get; set; }
}
