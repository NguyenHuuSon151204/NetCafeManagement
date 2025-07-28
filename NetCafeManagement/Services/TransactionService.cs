using BusinessObjects.Models;
using DataAccessLayer;
using Repositories;

namespace Services
{
    public class TransactionService
    {
        private readonly TransactionRepository _transactionRepo;

        public TransactionService(NetCafeContext context)
        {
            _transactionRepo = new TransactionRepository(context);
        }

        public IEnumerable<Transaction> GetAll() => _transactionRepo.GetAll();

        public Transaction? GetById(int id) => _transactionRepo.GetById(id);

        public void Create(Transaction obj) => _transactionRepo.Add(obj);

        public void Update(Transaction obj) => _transactionRepo.Update(obj);

        public void Delete(int id)
        {
            var entity = _transactionRepo.GetById(id);
            if (entity != null)
            {
                _transactionRepo.Delete(entity);
            }
        }
    }
}
