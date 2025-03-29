using Company.Rabeea.DAL.Models;

namespace Company.Rabeea.BLL.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(int id);
        Task AddAsync(T model);
        void Update(T model);
        void Delete(T model);

    }
}
