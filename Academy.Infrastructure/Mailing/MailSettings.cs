using Academy.Infrastructure.Mailing.AmazonSES;
using Academy.Infrastructure.Mailing.SendGrid;
using Academy.Infrastructure.Mailing.Smtp;

namespace Academy.Infrastructure.Mailing
{
    public class MailSettings
    {
        public string MailProvider { get; set; }

        public SmtpConfig SmtpConfig { get; set; }

        public AmazonSesConfig AmazonSesConfig { get; set; }

        public SendGridConfig SendGridConfig { get; set; }
    }
}
