using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public bool? IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public int? FailedAttempts { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}
