using Company.Rabeea.DAL.Models;

namespace Company.Rabeea.BLL.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public IEnumerable<T> GetAll();
        T? Get(int id);
        int Add(T model);
        int Update(T model);
        int Delete(T model);

    }
}
