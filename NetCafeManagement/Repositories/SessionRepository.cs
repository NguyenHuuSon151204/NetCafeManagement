using BusinessObjects.Models;
using DataAccessLayer;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
    public class UsageSessionRepository : GenericRepository<Session>, ISessionRepository
    {
        public UsageSessionRepository(NetCafeContext context) : base(context) { }

        public IEnumerable<Session> GetSessionsByCustomerId(int customerId)
        {
            return _context.Sessions
                .Where(s => s.CustomerId == customerId)
                .ToList();
        }

        public Session? GetActiveSessionByComputerId(int computerId)
        {
            return _context.Sessions
                .FirstOrDefault(s => s.ComputerId == computerId && s.EndTime == null);
        }
    }
}