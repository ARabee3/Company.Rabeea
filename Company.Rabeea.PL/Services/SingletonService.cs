namespace Company.Rabeea.PL.Services;

public class SingletonService : ISingletonService
{
    public SingletonService()
    {
        guid = Guid.NewGuid();
    }
    public Guid guid { get; set; }

    public string GetGuid()
    {
        return guid.ToString();
    }
}
