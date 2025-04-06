using Company.Rabeea.PL.Helpers;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Company.Rabeea.PL.Authentication;

public class TwilioService(IOptions<TwilioSettings> options) : ITwilioService
{
    private readonly TwilioSettings _options = options.Value;

    public MessageResource SendSms(Sms sms)
    {
        // Initialze Connection
        TwilioClient.Init(_options.AccountSID, _options.AuthToken);

        // Build and Return Message
        var message = MessageResource.Create(
            body: sms.Body,
            to: sms.To,
            from: _options.PhoneNumber
            );
        return message;
    }
}
