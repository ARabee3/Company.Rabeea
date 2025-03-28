namespace Company.Rabeea.PL.Services;

public interface IScopedService
{
    public Guid guid { get; set; }
    string GetGuid();
}
