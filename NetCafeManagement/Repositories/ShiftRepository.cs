using BusinessObjects.Models;
using DataAccessLayer;
using Repositories.Interfaces;

namespace Repositories
{
    public class ShiftRepository : GenericRepository<Shift>, IShiftRepository
    {
        public ShiftRepository(NetCafeContext context) : base(context) { }
    }
}
