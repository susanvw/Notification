using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace SvwDesign.Notification
{

    public class SendGridEmailSender : IEmailSender
    {
        private readonly EmailSenderOptions Options;

        public SendGridEmailSender(IOptions<EmailSenderOptions> options)
        {
            Options = options.Value;
        }

        public async Task<NotificationSentEvent> SendHtmlEmailAsync(string emailAddress, string subject, string htmlMessage)
        {
            return await Execute(subject, htmlMessage, emailAddress, true);
        }

        public async Task<NotificationSentEvent> SendTextEmailAsync(string emailAddress, string subject, string textMessage)
        {
            return await Execute(subject, textMessage, emailAddress, false);
        }

        public async Task<NotificationSentEvent> Execute(string subject, string message, string emailaddress, bool isHtml)
        {
            try
            {
                var client = new SendGridClient(Options.SendGridApiKey);
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(Options.SenderEmail, Options.SenderName),
                    Subject = subject,
                    PlainTextContent = isHtml ? string.Empty : message,
                    HtmlContent = isHtml ? message : string.Empty
                };
                msg.AddTo(new EmailAddress(emailaddress));

                msg.SetClickTracking(false, false);

                var response = await client.SendEmailAsync(msg);

                return new NotificationSentEvent
                {
                    StatusCode = response.StatusCode,
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    Message = "Successful."
                };
            }
            catch (Exception ex)
            {
                return new NotificationSentEvent
                {
                    IsSuccessStatusCode = false,
                    Message = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }
    }
}