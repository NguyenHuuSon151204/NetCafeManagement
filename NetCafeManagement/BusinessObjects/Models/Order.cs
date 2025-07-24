using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int? SessionId { get; set; }
        public int? ShiftId { get; set; }
        public DateTime? OrderTime { get; set; }
        public decimal? TotalAmount { get; set; }

        public virtual Session? Session { get; set; }
        public virtual Shift? Shift { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
