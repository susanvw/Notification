namespace SvwDesign.Notification.Options
{
    public class SendGridOptions 
    {
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SendGridApiKey { get; set; } = string.Empty;
    }
}
