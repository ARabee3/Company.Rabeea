using Company.Rabeea.DAL.Models;

namespace Company.Rabeea.BLL.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public IEnumerable<T> GetAll();
        T? Get(int id);
        void Add(T model);
        void Update(T model);
        void Delete(T model);

    }
}
