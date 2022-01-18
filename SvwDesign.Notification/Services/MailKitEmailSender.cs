using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text; 
using SvwDesign.Notification.Events;
using SvwDesign.Notification.Interfaces;
using SvwDesign.Notification.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SvwDesign.Notification.Services
{
    public class MailKitEmailSender : IEmailSender
    {
        private readonly MailKitOptions _options;
        private readonly IAttachmentBuilder _attachmentBuilder;

        public MailKitEmailSender(IOptions<MailKitOptions> options, IAttachmentBuilder attachmentBuilder)
        {
            _options = options.Value;
            _attachmentBuilder = attachmentBuilder;
        }

        public async Task<NotificationSentEvent> SendHtmlEmailAsync(string emailAddress, string subject, string htmlMessage, Dictionary<string, byte[]>? attachments)
        {
            return await Execute(subject, htmlMessage, emailAddress, TextFormat.Html, attachments);
        }

        public async Task<NotificationSentEvent> SendTextEmailAsync(string emailAddress, string subject, string textMessage, Dictionary<string, byte[]>? attachments)
        {
            return await Execute(subject, textMessage, emailAddress, TextFormat.Text, attachments);
        }

        private async Task<NotificationSentEvent> Execute(string subject, string message, string emailaddress, TextFormat format, Dictionary<string, byte[]>? attachments)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(MailboxAddress.Parse(_options.SenderEmail));
                email.To.Add(MailboxAddress.Parse(emailaddress));
                email.Subject = subject;


                // now create the multipart/mixed container to hold the message text and the
                // image attachment
                var multipart = new Multipart("mixed");

                var body = new TextPart(format) { Text = message };


                if (attachments is not null)
                {
                    foreach (var attachment in attachments)
                    {
                        var attachmentPart = new MimePart(MediaTypeNames.Application.Pdf)
                        {
                            Content = new MimeContent(await _attachmentBuilder.BuildAttachment(attachment.Value)),
                            ContentId = attachment.Key,
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = attachment.Key
                        };

                        multipart.Add(attachmentPart);
                    }
                }

                // now set the multipart/mixed as the message body
                email.Body = multipart;

                using SmtpClient? smtp = new(); 
                smtp.Connect(_options.Smtp, 587, SecureSocketOptions.None);

                smtp.Authenticate(_options.Username, _options.Password);

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




        public async Task<NotificationSentEvent> SendHtmlEmailAsync(string emailAddress, string subject, string htmlMessage, Dictionary<string, Stream>? attachments)
        {
            return await Execute(subject, htmlMessage, emailAddress, TextFormat.Html, attachments);
        }
        public async Task<NotificationSentEvent> SendTextEmailAsync(string emailAddress, string subject, string textMessage, Dictionary<string, Stream>? attachments)
        {
            return await Execute(subject, textMessage, emailAddress, TextFormat.Text, attachments);
        }
        private async Task<NotificationSentEvent> Execute(string subject, string message, string emailaddress, TextFormat format, Dictionary<string, Stream>? attachments)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(MailboxAddress.Parse(_options.SenderEmail));
                email.To.Add(MailboxAddress.Parse(emailaddress));
                email.Subject = subject;


                // now create the multipart/mixed container to hold the message text and the
                // image attachment
                var multipart = new Multipart("mixed");

                var body = new TextPart(format) { Text = message };
                multipart.Add(body);

                if (attachments is not null)
                {
                    foreach (var attachment in attachments)
                    {
                        var attachmentPart = new MimePart(MediaTypeNames.Application.Pdf)
                        {
                            Content = new MimeContent(attachment.Value),
                            ContentId = attachment.Key,
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = attachment.Key
                        };
                        multipart.Add(attachmentPart);
                    }
                }

                // now set the multipart/mixed as the message body
                email.Body = multipart;

                using SmtpClient? smtp = new();
                smtp.Connect(_options.Smtp, _options.Port, SecureSocketOptions.None);

                smtp.Authenticate(_options.Username, _options.Password);

                await smtp.SendAsync(email);

                smtp.Disconnect(true);

                return new NotificationSentEvent
                {
                    IsSuccessStatusCode = true,
                    StatusCode = System.Net.HttpStatusCode.OK,
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