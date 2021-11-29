using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SvwDesign.Notification
{
    public class SMSSender : ISMSSender
    {
        private readonly TwilioOptions Options;

        public SMSSender(IOptions<TwilioOptions> options)
        {
            Options = options.Value;
        }

        public async Task<NotificationSentEvent> SendSMSAsync(string to, string textMessage)
        {
            TwilioClient.Init(Options.AccountSid, Options.AuthToken);

            var message = await Task.Run(() => MessageResource.Create(
                body: textMessage,
                from: new Twilio.Types.PhoneNumber(Options.FromNumber),
                to: new Twilio.Types.PhoneNumber(to)
            ));

            return new NotificationSentEvent
            {
                IsSuccessStatusCode = !message.ErrorCode.HasValue,
                Message = message.ErrorCode.HasValue ? message.ErrorMessage : "Success",
                StatusCode = message.ErrorCode.HasValue ? System.Net.HttpStatusCode.BadRequest : System.Net.HttpStatusCode.OK
            };
        }
    }
}
