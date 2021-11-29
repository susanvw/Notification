using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Threading.Tasks;

namespace SvwDesign.Notification
{
    public class MailKitEmailSender : IEmailSender
    {
        private readonly EmailSenderOptions Options;

        public MailKitEmailSender(IOptions<EmailSenderOptions> options)
        {
            Options = options.Value;
        }

        public async Task<NotificationSentEvent> SendHtmlEmailAsync(string emailAddress, string subject, string htmlMessage)
        {
            return await Execute(subject, htmlMessage, emailAddress, TextFormat.Html);
        }

        public async Task<NotificationSentEvent> SendTextEmailAsync(string emailAddress, string subject, string textMessage)
        {
            return await Execute(subject, textMessage, emailAddress, TextFormat.Text);
        }

        private async Task<NotificationSentEvent> Execute(string subject, string message, string emailaddress, TextFormat format)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(MailboxAddress.Parse(Options.SenderEmail));
                email.To.Add(MailboxAddress.Parse(emailaddress));
                email.Subject = subject;
                email.Body = new TextPart(format) { Text = message };

                using SmtpClient? smtp = new(); 
                smtp.Connect(Options.Smtp, 587, SecureSocketOptions.StartTls);

                smtp.Authenticate(Options.Username, Options.Password);

                await smtp.SendAsync(email);

                smtp.Disconnect(true);

                return new NotificationSentEvent
                {
                    IsSuccessStatusCode = true,
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Successful."
                };
            }
            catch(Exception ex)
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