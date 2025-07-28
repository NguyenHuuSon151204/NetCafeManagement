using BusinessObjects.Models;
using DataAccessLayer;
using Repositories;
using Repositories.Interfaces;

namespace Services
{
    public class SessionService
    {
        private readonly SessionRepository _sessionRepo;

        public SessionService(NetCafeContext context)
        {
            _sessionRepo = new SessionRepository(context);
        }

        public IEnumerable<Session> GetAll() => _sessionRepo.GetAll();

        public Session? GetById(int id) => _sessionRepo.GetById(id);

        public void Create(Session obj) => _sessionRepo.Add(obj);

        public void Update(Session obj) => _sessionRepo.Update(obj);

        public void Delete(int id)
        {
            var entity = _sessionRepo.GetById(id);
            if (entity != null)
            {
                _sessionRepo.Delete(entity);
            }
        }
        public static void StartSession(int computerId, int customerId)
        {
            using var context = new NetCafeContext(); 

            var session = new Session
            {
                ComputerId = computerId,
                CustomerId = customerId,
                StartTime = DateTime.Now
            };
            context.Sessions.Add(session);

            var computer = context.Computers.Find(computerId);
            if (computer != null)
            {
                computer.Status = 2; // Đang sử dụng
            }

            context.SaveChanges();
        }
    }
}
