using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        IEnumerable<Transaction> GetTransactionsByDate(DateTime date);
        IEnumerable<Transaction> GetTransactionsByCustomer(int customerId);
    }

}
