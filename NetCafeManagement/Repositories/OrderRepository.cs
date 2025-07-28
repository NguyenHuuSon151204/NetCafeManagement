using BusinessObjects.Models;
using DataAccessLayer;
using Repositories.Interfaces;

namespace Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(NetCafeContext context) : base(context) { }
    }
}
