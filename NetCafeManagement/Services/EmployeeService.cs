using BusinessObjects.Models;
using DataAccessLayer;
using Repositories;

namespace Services
{
    public class EmployeeService
    {
        private readonly EmployeeRepository _employeeRepo;

        public EmployeeService(NetCafeContext context)
        {
            _employeeRepo = new EmployeeRepository(context);
        }

        public IEnumerable<Employee> GetAll() => _employeeRepo.GetAll();

        public Employee? GetById(int id) => _employeeRepo.GetById(id);

        public void Create(Employee obj) => _employeeRepo.Add(obj);

        public void Update(Employee obj) => _employeeRepo.Update(obj);

        public void Delete(int id)
        {
            var entity = _employeeRepo.GetById(id);
            if (entity != null)
            {
                _employeeRepo.Delete(entity);
            }
        }
    }
}
