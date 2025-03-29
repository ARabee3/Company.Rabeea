using Company.Rabeea.BLL.Interfaces;
using Company.Rabeea.DAL.Data.Contexts;
using Company.Rabeea.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Company.Rabeea.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly CompanyDbContext _context;

        public GenericRepository(CompanyDbContext context)
        {
            this._context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Employee))
            {
                return  (IEnumerable<T>) await _context.Employees.Include(E=> E.Department).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T?> GetAsync(int id)
        {
            if (typeof(T) == typeof(Employee))
            {
                return await _context.Employees.Include(E => E.Department).FirstOrDefaultAsync(E => E.Id == id) as T;
            }
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T model)
        {
            await _context.Set<T>().AddAsync(model);
        }
        public void Update(T model)
        {
            _context.Set<T>().Update(model);
        }
        public void Delete(T model)
        {
            _context.Set<T>().Remove(model);
        }

    }
}
