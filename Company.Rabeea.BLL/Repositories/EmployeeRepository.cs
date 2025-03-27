using Company.Rabeea.BLL.Interfaces;
using Company.Rabeea.DAL.Data.Contexts;
using Company.Rabeea.DAL.Models;

namespace Company.Rabeea.BLL.Repositories
{

    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext context) : base(context)
        {
            this._context = context;
        }

        public IEnumerable<Employee> GetByName(string input)
        {
            return _context.Employees.Where(E => E.Name.ToLower().Contains(input.ToLower()));
        }
        

       
    }
}
