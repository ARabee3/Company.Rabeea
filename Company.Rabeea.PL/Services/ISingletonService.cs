namespace Company.Rabeea.PL.Services;

public interface ISingletonService
{
    public Guid guid { get; set; }
    string GetGuid();
}
