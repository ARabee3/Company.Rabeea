using Company.Rabeea.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Rabeea.BLL.Interfaces
{
    public interface IDepartmentRepository 
    {
        public IEnumerable<Department> GetAll();
        Department? Get(int id);
        int Add(Department department);
        int Update(Department department);
        int Delete(Department department);
    }
}
