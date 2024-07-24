using Academy.Application.Common.Mailing;
using Academy.Application.Contracts.Persistence;
using Academy.Domain.Mailing;
using ManagedCode.Communication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace Academy.Infrastructure.Mailing.Smtp
{
    public class SmtpMailService : BaseEmailHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IEmailHistoryRepository _emailHistoryRepository;

        public SmtpMailService(IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment,
            IEmailHistoryRepository emailHistoryRepository) : base(hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _emailHistoryRepository = emailHistoryRepository;
        }

        protected async override Task<Result<bool>> SendMailAsync(MailRequest mailRequest)
        {
            try
            {
                SmtpConfig smtpConfig = _configuration.GetSection(nameof(MailSettings)).Get<MailSettings>()!.SmtpConfig;
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                if (mailRequest.To != null && mailRequest.To.Count > 0)
                {
                    message.To.Add(string.Join(",", mailRequest.To));
                }
                if (mailRequest.Cc != null && mailRequest.Cc.Count > 0)
                {
                    message.CC.Add(string.Join(",", mailRequest.Cc));
                }
                if (mailRequest.Bcc != null && mailRequest.Bcc.Count > 0)
                {
                    message.Bcc.Add(string.Join(",", mailRequest.Bcc));
                }
                message.From = new MailAddress(smtpConfig.From, smtpConfig.DisplayName);
                message.Subject = mailRequest.Subject;
                message.Body = mailRequest.Body;
                if (mailRequest.AttachmentData != null && mailRequest.AttachmentData.Count > 0)
                {
                    foreach (var i in mailRequest.AttachmentData)
                    {
                        var attachment = new Attachment(new MemoryStream(i.Value), i.Key);
                        message.Attachments.Add(attachment);
                    }
                }

                var smtpClient = new SmtpClient();
                if (!string.IsNullOrEmpty(smtpConfig.Host))
                    smtpClient.Host = smtpConfig.Host; //Define Host or Sending URL

                if (smtpConfig.Port > 0)
                    smtpClient.Port = smtpConfig.Port;

                smtpClient.EnableSsl = true;
                if (!string.IsNullOrEmpty(smtpConfig.UserName) && !string.IsNullOrEmpty(smtpConfig.Password))
                    smtpClient.Credentials = new System.Net.NetworkCredential(smtpConfig.UserName, smtpConfig.Password);

                smtpClient.Send(message);
                await _emailHistoryRepository.SaveEmailAsync(mailRequest.To, mailRequest.Body, smtpConfig.From, true, mailRequest.Cc, mailRequest.Bcc, mailRequest.Subject);
                return Result<bool>.Succeed(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex);
            }
        }
    }
}
