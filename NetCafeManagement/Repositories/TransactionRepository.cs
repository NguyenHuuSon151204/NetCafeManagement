using BusinessObjects.Models;
using DataAccessLayer;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(NetCafeContext context) : base(context) { }

        public IEnumerable<Transaction> GetTransactionsByDate(DateTime date)
        {
            return _context.Transactions
                .Where(t => t.CreatedAt.HasValue && t.CreatedAt.Value.Date == date.Date)
                .ToList();
        }

        public IEnumerable<Transaction> GetTransactionsByCustomer(int customerId)
        {
            return _context.Transactions
                .Where(t => t.CustomerId == customerId)
                .ToList();
        }
    }
}