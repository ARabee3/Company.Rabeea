using Company.Rabeea.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Rabeea.BLL.Interfaces
{
    public interface IEmployeeRepository
    {
        public IEnumerable<Employee> GetAll();
        Employee? Get(int id);
        int Add(Employee department);
        int Update(Employee department);
        int Delete(Employee department);
    }
}
