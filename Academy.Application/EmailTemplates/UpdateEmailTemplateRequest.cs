using Academy.Application.Common.Exceptions;
using Academy.Application.Contracts.Persistence;
using Mapster;

namespace Academy.Application.EmailTemplates
{
    public class UpdateEmailTemplateRequest : IRequest<Result<EmailTemplateDto>>
    {
        public string TemplateCode { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string? To { get; set; }

        public string? CC { get; set; }

        public string? BCC { get; set; }

        public string Subject { get; set; } = default!;

        public string Body { get; set; } = default!;

        public DefaultIdType Id { get; set; }

    }

    public class UpdateEmailTemplateRequestValidator : CustomValidator<UpdateEmailTemplateRequest>
    {
        public UpdateEmailTemplateRequestValidator(IEmailTemplateRepository emailTemplateRepository)
        {
            RuleFor(p => p.TemplateCode)
           .NotEmpty()
               .WithMessage(DbRes.T("EmailTemplateCodeRequiredMsg"))
           .MustAsync(async (emailtemplate, templateCode, _) => !await emailTemplateRepository.CheckEmailTemplateExist(templateCode, emailtemplate.Id))
               .WithMessage((_, templateCode) => DbRes.T("EmailTemplateAlreadyExistMsg"));

            RuleFor(p => p.Name)
          .NotEmpty()
              .WithMessage(DbRes.T("EmailTemplateNameRequiredMsg"));

            RuleFor(p => p.Subject)
            .NotEmpty()
                .WithMessage(DbRes.T("EmailTemplateSubjectRequiredMsg"));

            RuleFor(p => p.Body)
           .NotEmpty()
               .WithMessage(DbRes.T("EmailTemplateBodyRequiredMsg"));
        }
    }

    public class UpdateEmailTemplateRequestHandler(IEmailTemplateRepository emailTemplateRepository) : IRequestHandler<UpdateEmailTemplateRequest, Result<EmailTemplateDto>>
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository = emailTemplateRepository;

        public async Task<Result<EmailTemplateDto>> Handle(UpdateEmailTemplateRequest emailTemplate, CancellationToken cancellationToken)
        {
            var emailTemplateData = await _emailTemplateRepository.GetByIdAsync(emailTemplate.Id);
            if (emailTemplateData != null)
            {
                emailTemplateData.TemplateCode = emailTemplate.TemplateCode;
                emailTemplateData.Name = emailTemplate.Name;
                emailTemplateData.Subject = emailTemplate.Subject;
                emailTemplateData.Body = emailTemplate.Body;
                emailTemplateData.To = emailTemplate.To;
                emailTemplateData.CC = emailTemplate.CC;
                emailTemplateData.BCC = emailTemplate.BCC;

                await _emailTemplateRepository.UpdateAsync(emailTemplateData);
                return Result.Succeed(emailTemplateData.Adapt<EmailTemplateDto>());
            }
            else
            {
                throw new CustomException(DbRes.T("EmailTemplateDoesNotExistMsg"));
            }
        }
    }
}
