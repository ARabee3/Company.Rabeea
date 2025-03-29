namespace Company.Rabeea.BLL.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IDepartmentRepository DepartmentRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        Task<int> CompleteAsync();
    }

}
