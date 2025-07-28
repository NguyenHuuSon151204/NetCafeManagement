using BusinessObjects.Models;
using DataAccessLayer;
using Repositories;

namespace Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepo;

        public OrderService(NetCafeContext context)
        {
            _orderRepo = new OrderRepository(context);
        }

        public IEnumerable<Order> GetAll() => _orderRepo.GetAll();

        public Order? GetById(int id) => _orderRepo.GetById(id);

        public void Create(Order obj) => _orderRepo.Add(obj);

        public void Update(Order obj) => _orderRepo.Update(obj);

        public void Delete(int id)
        {
            var entity = _orderRepo.GetById(id);
            if (entity != null)
            {
                _orderRepo.Delete(entity);
            }
        }
    }
}
