using System.Net;

namespace SvwDesign.Notification.Events
{
    public class NotificationSentEvent 
    { 
        public HttpStatusCode StatusCode { get; set; } 
        public bool IsSuccessStatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
