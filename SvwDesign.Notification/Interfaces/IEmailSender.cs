using SvwDesign.Notification.Events;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SvwDesign.Notification.Interfaces
{
    public interface IEmailSender
    {
        Task<NotificationSentEvent> SendHtmlEmailAsync(string emailAddress, string subject, string htmlMessage, Dictionary<string, byte[]>? attachments);
        Task<NotificationSentEvent> SendHtmlEmailAsync(string emailAddress, string subject, string htmlMessage, Dictionary<string, Stream>? attachments);
        Task<NotificationSentEvent> SendTextEmailAsync(string emailAddress, string subject, string textMessage, Dictionary<string, byte[]>? attachments);
        Task<NotificationSentEvent> SendTextEmailAsync(string emailAddress, string subject, string textMessage, Dictionary<string, Stream>? attachments);
    }
}