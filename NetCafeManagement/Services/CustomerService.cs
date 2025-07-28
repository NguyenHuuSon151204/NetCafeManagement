using BusinessObjects.Models;
using DataAccessLayer;
using Repositories;

namespace Services
{
    public class CustomerService
    {
        private readonly CustomerRepository _customerRepo;

        public CustomerService(NetCafeContext context)
        {
            _customerRepo = new CustomerRepository(context);
        }

        public IEnumerable<Customer> GetAll() => _customerRepo.GetAll();

        public Customer? GetById(int id) => _customerRepo.GetById(id);

        public void Create(Customer obj) => _customerRepo.Add(obj);

        public void Update(Customer obj) => _customerRepo.Update(obj);

        public void Delete(int id)
        {
            var entity = _customerRepo.GetById(id);
            if (entity != null)
            {
                _customerRepo.Delete(entity);
            }
        }
    }
}
