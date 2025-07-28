using BusinessObjects.Models;
using DataAccessLayer;
using Repositories.Interfaces;

namespace Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(NetCafeContext context) : base(context) { }
    }
}
