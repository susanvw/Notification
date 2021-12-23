namespace SvwDesign.Notification
{
    public class EmailSenderOptions
    {
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string? Smtp { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? SendGridApiKey { get; set; }
    }
}
