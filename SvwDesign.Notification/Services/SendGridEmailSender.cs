using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using SvwDesign.Notification.Events;
using SvwDesign.Notification.Interfaces;
using SvwDesign.Notification.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SvwDesign.Notification.Services
{

    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridOptions _options;
        private readonly IAttachmentBuilder _attachmentBuilder;

        public SendGridEmailSender(IOptions<SendGridOptions> options, IAttachmentBuilder attachmentBuilder)
        {
            _options = options.Value;
            _attachmentBuilder = attachmentBuilder;
        }

        public async Task<NotificationSentEvent> SendHtmlEmailAsync(string emailAddress, string subject, string htmlMessage, Dictionary<string, byte[]>? attachments)
        {
            return await Execute(subject, htmlMessage, emailAddress, true, attachments);
        }

        public async Task<NotificationSentEvent> SendTextEmailAsync(string emailAddress, string subject, string textMessage, Dictionary<string, byte[]>? attachments)
        {
            return await Execute(subject, textMessage, emailAddress, false, attachments);
        }

        public async Task<NotificationSentEvent> SendHtmlEmailAsync(string emailAddress, string subject, string htmlMessage, Dictionary<string, Stream>? attachments)
        {
            return await Execute(subject, htmlMessage, emailAddress, true, null, attachments);
        }

        public async Task<NotificationSentEvent> SendTextEmailAsync(string emailAddress, string subject, string textMessage, Dictionary<string, Stream>? attachments)
        {
            return await Execute(subject, textMessage, emailAddress, false, null, attachments);
        }


        private async Task<NotificationSentEvent> Execute(string subject, string message, string emailaddress, bool isHtml, Dictionary<string, byte[]>? byteAttachments = null!, Dictionary<string, Stream>? streamAttachments = null!)
        {
            try
            {
                var client = new SendGridClient(_options.SendGridApiKey);

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(_options.SenderEmail, _options.SenderName),
                    Subject = subject,
                    PlainTextContent = isHtml ? string.Empty : message,
                    HtmlContent = isHtml ? message : string.Empty
                };

                if (byteAttachments is not null)
                {
                    foreach (var attachment in byteAttachments)
                    {
                        await msg.AddAttachmentAsync(attachment.Key, await _attachmentBuilder.BuildAttachment(attachment.Value));
                    }
                }

                if (streamAttachments is not null)
                {
                    foreach (var attachment in streamAttachments)
                    {
                        await msg.AddAttachmentAsync(attachment.Key, attachment.Value);
                    }
                }

                msg.AddTo(new EmailAddress(emailaddress));

                msg.SetClickTracking(false, false);

                var response = await client.SendEmailAsync(msg);

                return new NotificationSentEvent
                {
                    StatusCode = response.StatusCode,
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    Message = await response.Body.ReadAsStringAsync()
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