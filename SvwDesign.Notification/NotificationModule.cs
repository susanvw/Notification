using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SvwDesign.Notification
{
    public static class NotificationModule
    {
        public static void AddTwilioModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<TwilioOptions>(configuration.GetSection(nameof(TwilioOptions)));
            services.AddTransient<ISMSSender, SMSSender>();
        }

        public static void AddMailKitModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<EmailSenderOptions>(configuration.GetSection(nameof(EmailSenderOptions)));
            services.AddTransient<IEmailSender, MailKitEmailSender>();
        }

        public static void AddSendGridModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<EmailSenderOptions>(configuration.GetSection(nameof(EmailSenderOptions)));
            services.AddTransient<IEmailSender, SendGridEmailSender>();
        }
    }
}
