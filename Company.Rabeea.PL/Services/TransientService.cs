namespace Company.Rabeea.PL.Services;

public class TransientService : ITransientService
{
    public TransientService()
    {
        guid = Guid.NewGuid();
    }
    public Guid guid { get; set; }

    public string GetGuid()
    {
        return guid.ToString();
    }
}
