using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SvwDesign.Notification
{
    public static class NotificationModule
    {
        public static void AddNotificationModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<EmailSenderOptions>(configuration.GetSection(nameof(EmailSenderOptions)));
            services.Configure<TwilioOptions>(configuration.GetSection(nameof(TwilioOptions)));

            // add services
            services.AddTransient<IEmailSender, MailKitEmailSender>();
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            services.AddTransient<ISMSSender, SMSSender>();
        }
    }
}
