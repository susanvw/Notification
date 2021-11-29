using System.Threading.Tasks;

namespace SvwDesign.Notification
{
    public interface IEmailSender
    {
        Task<NotificationSentEvent> SendHtmlEmailAsync(string emailAddress, string subject, string htmlMessage);
        Task<NotificationSentEvent> SendTextEmailAsync(string emailAddress, string subject, string textMessage);
    }
}