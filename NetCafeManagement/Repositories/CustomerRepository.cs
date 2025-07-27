using BusinessObjects.Models;
using DataAccessLayer;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(NetCafeContext context) : base(context) { }

        public IEnumerable<Customer> GetCustomersByName(string name)
        {
            return _context.Customers
                .Where(c => c.Name.Contains(name))
                .ToList();
        }
    }
}
