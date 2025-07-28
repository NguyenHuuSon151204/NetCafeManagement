using BusinessObjects.Models;
using DataAccessLayer;
using Repositories;

namespace Services
{
    public class ShiftService
    {
        private readonly ShiftRepository _shiftRepo;

        public ShiftService(NetCafeContext context)
        {
            _shiftRepo = new ShiftRepository(context);
        }

        public IEnumerable<Shift> GetAll() => _shiftRepo.GetAll();

        public Shift? GetById(int id) => _shiftRepo.GetById(id);

        public void Create(Shift obj) => _shiftRepo.Add(obj);

        public void Update(Shift obj) => _shiftRepo.Update(obj);

        public void Delete(int id)
        {
            var entity = _shiftRepo.GetById(id);
            if (entity != null)
            {
                _shiftRepo.Delete(entity);
            }
        }
    }
}
