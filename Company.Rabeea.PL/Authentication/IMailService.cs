using Company.Rabeea.PL.Helpers;

namespace Company.Rabeea.PL.Authentication;

public interface IMailService
{
    public void SendEmail(Email email);
}
