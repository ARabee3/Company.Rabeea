using Company.Rabeea.BLL.Interfaces;
using Company.Rabeea.DAL.Data.Contexts;
using Company.Rabeea.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Rabeea.BLL.Repositories
{

    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Employee>> GetByNameAsync(string input)
        {
            return await _context.Employees.Where(E => E.Name.ToLower().Contains(input.ToLower())).ToListAsync();
        }
        

       
    }
}
