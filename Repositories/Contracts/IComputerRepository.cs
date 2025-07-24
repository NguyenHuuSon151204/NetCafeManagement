using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    // Contracts/IComputerRepository.cs
    public interface IComputerRepository : IBaseRepository<Computer>
    {
        Task<IEnumerable<Computer>> GetComputersByStatusAsync(string status);
    }
}
