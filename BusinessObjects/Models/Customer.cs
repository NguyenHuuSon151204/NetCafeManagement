using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Sessions = new HashSet<Session>();
            Transactions = new HashSet<Transaction>();
        }

        public int CustomerId { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public decimal? Balance { get; set; }
        public DateTime? JoinDate { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
