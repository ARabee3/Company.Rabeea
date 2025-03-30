using System.Net;
using System.Net.Mail;

namespace Company.Rabeea.PL.Helpers;

public static class EmailSettings
{
    public static bool SendEmail(Email email)
    {
        try
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("arabeea7104@gmail.com", "fymlddbczaungidk");
            
            client.Send("arabeea7104@gmail.com", email.To, email.Subject, email.Body);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
