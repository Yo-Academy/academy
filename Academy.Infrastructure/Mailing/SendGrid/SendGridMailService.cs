using Academy.Application.Common.Mailing;
using Academy.Application.Contracts.Persistence;
using Academy.Domain.Mailing;
using ManagedCode.Communication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Academy.Infrastructure.Mailing.SendGrid
{
    public class SendGridMailService : BaseEmailHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IEmailHistoryRepository _emailHistoryRepository;

        public SendGridMailService(IConfiguration configuration,
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
                SendGridConfig sendGridConfig = _configuration.GetSection(nameof(MailSettings)).Get<MailSettings>()!.SendGridConfig;
                var apiKey = sendGridConfig.ApiKey;
                var client = new SendGridClient(apiKey);
                SendGridMessage msg = new SendGridMessage();
                if (mailRequest.To != null && mailRequest.To.Count > 0)
                {
                    msg.AddTos(mailRequest.To.Select(x => new EmailAddress(x)).ToList());
                }
                if (mailRequest.Cc != null && mailRequest.Cc.Count > 0)
                {
                    msg.AddCcs(mailRequest.Cc.Select(x => new EmailAddress(x)).ToList());
                }
                if (mailRequest.Bcc != null && mailRequest.Bcc.Count > 0)
                {
                    msg.AddBccs(mailRequest.Bcc.Select(x => new EmailAddress(x)).ToList());
                }
                msg.From = new EmailAddress(sendGridConfig.From, sendGridConfig.DisplayName);
                msg.Subject = mailRequest.Subject;
                msg.HtmlContent = mailRequest.Body;
                if (mailRequest.AttachmentData != null && mailRequest.AttachmentData.Count > 0)
                {
                    foreach (var i in mailRequest.AttachmentData)
                    {
                        await msg.AddAttachmentAsync(i.Key, new MemoryStream(i.Value));
                    }
                }
                var response = await client.SendEmailAsync(msg);
                await _emailHistoryRepository.SaveEmailAsync(mailRequest.To, mailRequest.Body, sendGridConfig.From, true, mailRequest.Cc, mailRequest.Bcc, mailRequest.Subject);
                return Result<bool>.Succeed(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex);
            }
        }
    }
}
