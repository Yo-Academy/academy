using Academy.Application.Common.Mailing;
using Academy.Application.Contracts.Persistence;
using Academy.Application.Features.Mailing.Dto;
using Academy.Domain.Mailing;
using Microsoft.AspNetCore.Http;

namespace Academy.Application.Features.Mailing
{
    public class MailSendRequest : IRequest<bool>
    {
        public List<string> To { get; set; }

        public IFormFile? Attachment { get; set; }
    }

    public class MailSendRequestHandler(IEmailHelper emailHelper,
        IEmailTemplateRepository emailTemplateRepository) : IRequestHandler<MailSendRequest, bool>
    {
        private readonly IEmailHelper _emailHelper = emailHelper;
        private readonly IEmailTemplateRepository _emailTemplateRepository = emailTemplateRepository;

        public async Task<bool> Handle(MailSendRequest request, CancellationToken cancellationToken)
        {
            try
            {
                RegisterUserEmailDto emailModel = new RegisterUserEmailDto()
                {
                    Email = "test@test.com",
                    UserName = "Test",
                    Url = "test.com"
                };
                var emailTemplate = await _emailTemplateRepository.GetEmailTemplateByCodeAsync("CNFEM");
                var body = _emailHelper.GenerateEmailTemplate(emailTemplate.Body, emailModel);
                if (!string.IsNullOrEmpty(emailTemplate.To))
                {
                    request.To.Add(emailTemplate.To);
                }
                MailRequest mailRequest = new MailRequest() { To = request.To, Bcc = emailTemplate.BCC?.Split(',')?.ToList() ?? null, Cc = emailTemplate.CC?.Split(',')?.ToList() ?? null, Subject = emailTemplate.Subject, Body = body };
                if (request.Attachment != null)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        mailRequest.AttachmentData = new Dictionary<string, byte[]>();
                        request.Attachment.CopyTo(memoryStream);
                        mailRequest.AttachmentData.Add(request.Attachment.FileName, memoryStream.ToArray());
                    }
                }
                var res = await _emailHelper.SendAsync(mailRequest);
                return res;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
