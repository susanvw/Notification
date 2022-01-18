using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SvwDesign.Notification.Interfaces;
using SvwDesign.Notification.Options;
using SvwDesign.Notification.Services;

namespace SvwDesign.Notification
{
    public static class DependencyInjection
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
            services.Configure<MailKitOptions>(configuration.GetSection(nameof(MailKitOptions)));
            services.AddTransient<IAttachmentBuilder, AttachmentBuilder>();
            services.AddTransient<IEmailSender, MailKitEmailSender>();
        }

        public static void AddSendGridModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<SendGridOptions>(configuration.GetSection(nameof(SendGridOptions)));
            services.AddTransient<IAttachmentBuilder, AttachmentBuilder>();
            services.AddTransient<IEmailSender, SendGridEmailSender>();
        }
    }
}
