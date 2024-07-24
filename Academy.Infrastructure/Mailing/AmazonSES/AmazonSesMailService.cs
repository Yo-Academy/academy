using Academy.Application.Common.Mailing;
using Academy.Application.Contracts.Persistence;
using Academy.Domain.Mailing;
using Academy.Infrastructure.Mailing.Smtp;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using ManagedCode.Communication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Academy.Infrastructure.Mailing.AmazonSES
{
    public class AmazonSesMailService : BaseEmailHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IEmailHistoryRepository _emailHistoryRepository;

        public AmazonSesMailService(IConfiguration configuration,
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
                AmazonSesConfig sesConfig = _configuration.GetSection(nameof(MailSettings)).Get<MailSettings>()!.AmazonSesConfig;
                SendEmailResponse emailResponse = new SendEmailResponse();

                string accessKey = sesConfig.UserName;
                string secretKey = sesConfig.Password;
                RegionEndpoint regionEndpoint = RegionEndpoint.GetBySystemName(sesConfig.RegionEndpoint);
                using (var client = new AmazonSimpleEmailServiceClient(accessKey, secretKey, regionEndpoint))
                {
                    var bodyBuilder = new BodyBuilder();

                    bodyBuilder.HtmlBody = mailRequest.Body;

                    if (mailRequest.AttachmentData != null && mailRequest.AttachmentData.Count > 0)
                    {
                        foreach (var i in mailRequest.AttachmentData)
                        {
                            bodyBuilder.Attachments.Add(i.Key, new MemoryStream(i.Value));
                        }
                    }

                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(new MailboxAddress(sesConfig.DisplayName, sesConfig.From));
                    if (mailRequest.To != null && mailRequest.To.Count > 0)
                    {
                        mimeMessage.To.AddRange(mailRequest.To.Select(toAddress => new MailboxAddress("", toAddress)));
                    }
                    if (mailRequest.Cc != null && mailRequest.Cc.Count > 0)
                    {
                        mimeMessage.Cc.AddRange(mailRequest.Cc.Select(ccAddress => new MailboxAddress("", ccAddress)));
                    }
                    if (mailRequest.Bcc != null && mailRequest.Bcc.Count > 0)
                    {
                        mimeMessage.Bcc.AddRange(mailRequest.Bcc.Select(bccAddress => new MailboxAddress("", bccAddress)));
                    }

                    mimeMessage.Subject = mailRequest.Subject;
                    mimeMessage.Body = bodyBuilder.ToMessageBody();
                    using (var messageStream = new MemoryStream())
                    {
                        await mimeMessage.WriteToAsync(messageStream);
                        var sendRequest = new SendRawEmailRequest { RawMessage = new RawMessage(messageStream) };
                        var response = await client.SendRawEmailAsync(sendRequest);
                    }
                    await _emailHistoryRepository.SaveEmailAsync(mailRequest.To, mailRequest.Body, sesConfig.From, true, mailRequest.Cc, mailRequest.Bcc, mailRequest.Subject);
                    return Result<bool>.Succeed(true);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex);
            }

        }
    }
}
