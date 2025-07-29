using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCafeManagement_WDF.ViewModels
{
    public class ComputerViewModel
    {
        public int ComputerId { get; set; }
        public string Name { get; set; }
        public string Tier { get; set; }
        public decimal HourlyRate { get; set; }
        public string HourlyRateDisplay => $"{HourlyRate:N0} VNĐ";
        public byte? Status { get; set; }
        public string StatusText =>
            Status switch
            {
                1 => "Rảnh",
                2 => "Đang sử dụng",
                3 => "Bảo trì",
                _ => "Không xác định"
            };
    }
}