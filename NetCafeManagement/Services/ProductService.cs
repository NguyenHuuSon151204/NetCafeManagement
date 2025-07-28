using BusinessObjects.Models;
using DataAccessLayer;
using Repositories;

namespace Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepo;

        public ProductService(NetCafeContext context)
        {
            _productRepo = new ProductRepository(context);
        }

        public IEnumerable<Product> GetAll() => _productRepo.GetAll();

        public Product? GetById(int id) => _productRepo.GetById(id);

        public void Create(Product obj) => _productRepo.Add(obj);

        public void Update(Product obj) => _productRepo.Update(obj);

        public void Delete(int id)
        {
            var entity = _productRepo.GetById(id);
            if (entity != null)
            {
                _productRepo.Delete(entity);
            }
        }
    }
}
