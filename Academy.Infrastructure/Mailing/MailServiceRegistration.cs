using Academy.Application.Common.Mailing;
using Academy.Infrastructure.Mailing.AmazonSES;
using Academy.Infrastructure.Mailing.SendGrid;
using Academy.Infrastructure.Mailing.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Academy.Infrastructure.Mailing
{
    internal static class MailServiceRegistration
    {
        internal static IServiceCollection AddMailService(this IServiceCollection services, IConfiguration config)
        {
            MailSettings mailSettings = config.GetSection(nameof(MailSettings)).Get<MailSettings>();
            if (mailSettings.MailProvider == "Smtp")
            {
                services.AddSmtpMailService();
            }
            else if (mailSettings.MailProvider == "SES")
            {
                services.AddAmazonSesMailService();
            }
            else if (mailSettings.MailProvider == "SendGrid")
            {
                services.AddSendGridMailService();
            }
            return services;
        }

        public static IServiceCollection AddSmtpMailService(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient<IEmailHelper, SmtpMailService>();
        }

        public static IServiceCollection AddAmazonSesMailService(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient<IEmailHelper, AmazonSesMailService>();
        }

        public static IServiceCollection AddSendGridMailService(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient<IEmailHelper, SendGridMailService>();
        }
    }


}
