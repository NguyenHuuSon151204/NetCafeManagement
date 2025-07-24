using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Shift
    {
        public Shift()
        {
            Orders = new HashSet<Order>();
        }

        public int ShiftId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal StartCash { get; set; }
        public decimal? EndCash { get; set; }
        public string? Notes { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
