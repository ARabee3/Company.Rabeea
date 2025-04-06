using Company.Rabeea.PL.Helpers;
using Twilio.Rest.Api.V2010.Account;

namespace Company.Rabeea.PL.Authentication;

public interface ITwilioService
{
    public MessageResource SendSms(Sms sms); 
}
