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

    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext context)
        {
            this._context = context;
        }
        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }
        public Employee? Get(int id)
        {
            return _context.Employees.Find(id);
        }
        public int Add(Employee department)
        {
            _context.Employees.Add(department);
            return _context.SaveChanges();
        }
        public int Update(Employee department)
        {
            _context.Employees.Update(department);
            return _context.SaveChanges();
        }
        public int Delete(Employee department)
        {
            _context.Employees.Remove(department);
            return _context.SaveChanges();
        }
    }
}
