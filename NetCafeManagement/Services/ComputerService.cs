using BusinessObjects.Models;
using DataAccessLayer;
using Repositories;
using Repositories.Interfaces;

namespace Services
{
    public class ComputerService
    {
        private readonly ComputerRepository _computerRepo;

        public ComputerService(NetCafeContext context)
        {
            _computerRepo = new ComputerRepository(context);
        }

        public IEnumerable<Computer> GetAll() => _computerRepo.GetAll();

        public Computer? GetById(int id) => _computerRepo.GetById(id);

        public void Create(Computer obj) => _computerRepo.Add(obj);

        public void Update(Computer obj) => _computerRepo.Update(obj);

        public void Delete(int id)
        {
            var entity = _computerRepo.GetById(id);
            if (entity != null)
            {
                _computerRepo.Delete(entity);
            }
        }
        public List<Computer> GetAllComputers()
        {
            return _computerRepo.GetAll().ToList();
        }

    }
}
