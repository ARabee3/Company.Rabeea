using Company.Rabeea.BLL.Interfaces;
using Company.Rabeea.DAL.Data.Contexts;
using Company.Rabeea.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Rabeea.BLL.Repositories
{

    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly CompanyDbContext _context;
        public DepartmentRepository(CompanyDbContext companyDbContext)
        {
            _context = companyDbContext;
        }
        public IEnumerable<Department> GetAll()
        {
            return _context.Departments.ToList();
        }
        public Department? Get(int id)
        {
            return _context.Departments.Find(id);
        }

        public int Add(Department department)
        {
            _context.Add(department);
            return _context.SaveChanges();
        }
        public int Update(Department department)
        {
            _context.Update(department);
            return _context.SaveChanges();
        }

        public int Delete(Department department)
        {
            _context.Remove(department);
            return _context.SaveChanges();
        }
        
    }
}
