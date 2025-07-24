using BusinessObjects.Models;
using DataAccessLayer;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Repositories
{
    // Repositories/ComputerRepository.cs
    public class ComputerRepository : BaseRepository<Computer>, IComputerRepository
    {
        public ComputerRepository(NetCafeContext context) : base(context) { }

        public async Task<IEnumerable<Computer>> GetComputersByStatusAsync(string status)
        {
            if (byte.TryParse(status, out byte parsedStatus))
            {
                return await _context.Computers
                                     .Where(c => c.Status == parsedStatus)
                                     .ToListAsync();
            }
            else
            {
                return await _context.Computers.ToListAsync();
            }
        }
    }
}
