using System.Threading.Tasks;

namespace SvwDesign.Notification
{
    public interface ISMSSender
    {
        Task<NotificationSentEvent> SendSMSAsync(string to, string textMessage);
    }
}
