using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ActivityLogViewModel
    {
        public DateTime Timestamp { get; set; }
        public string Username { get; set; }
        public string ActivityType { get; set; }
        public string ActivityTypeText
        {
            get
            {
                return ActivityType switch
                {
                    "Login" => "Đăng nhập",
                    "Logout" => "Đăng xuất",
                    "Payment" => "Thanh toán",
                    _ => "Khác"
                };
            }
        }
        public string? ComputerName { get; set; }
        public string? Duration { get; set; }
        public string? Amount { get; set; }
    }

}
