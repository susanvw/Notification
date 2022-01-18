using SvwDesign.Notification.Events;
using System.Threading.Tasks;

namespace SvwDesign.Notification.Interfaces
{
    public interface ISMSSender
    {
        Task<NotificationSentEvent> SendSMSAsync(string to, string textMessage);
    }
}
