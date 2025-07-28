using BusinessObjects.Models;
using DataAccessLayer;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;


namespace Repositories
{
    public class ComputerRepository : GenericRepository<Computer>, IComputerRepository
    {
        public ComputerRepository(NetCafeContext context) : base(context) { }

        public IEnumerable<Computer> GetAvailableComputers()
        {
            return _context.Computers.Where(c => c.Status == 1).ToList();
        }
    }
}