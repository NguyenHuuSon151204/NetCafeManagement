using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessLayer;


namespace Services
{


    public class ActivityLogService
    {
        private readonly NetCafeContext _context;

        public ActivityLogService(NetCafeContext context)
        {
            _context = context;
        }

        public IEnumerable<ActivityLogViewModel> GetAllLogs()
        {
            return _context.Sessions.Select(s => new ActivityLogViewModel
            {
                Timestamp = s.StartTime,

                Username = s.Customer != null
                    ? s.Customer.Name
                    : "Unknown",

                ActivityType = s.EndTime == null
                    ? "Login"
                    : "Payment",

                ComputerName = s.Computer != null
                    ? s.Computer.Name
                    : "Unknown",

                Duration = s.EndTime != null
                    ? (s.EndTime.Value - s.StartTime).ToString(@"hh\:mm\:ss")
                    : string.Empty,

                Amount = s.TotalAmount.HasValue
                    ? $"{s.TotalAmount.Value:N0} VNĐ"
                    : "0 VNĐ"
            }).ToList();
        }


        public IEnumerable<ActivityLogViewModel> FilterLogs(string type, DateTime? from, DateTime? to)
        {
            var logs = GetAllLogs();

            if (type != "All")
                logs = logs.Where(l => l.ActivityType == type);

            if (from.HasValue)
                logs = logs.Where(l => l.Timestamp.Date >= from.Value.Date);

            if (to.HasValue)
                logs = logs.Where(l => l.Timestamp.Date <= to.Value.Date);

            return logs;
        }
    }

}
