using Academy.Application.Common.Exceptions;
using Academy.Application.Contracts.Persistence;
using Academy.Domain.Entities;
using Mapster;

namespace Academy.Application.EmailTemplates
{
    public class CreateEmailTemplateRequest : IRequest<Result<EmailTemplateDto>>
    {
        public string TemplateCode { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string? To { get; set; }

        public string? CC { get; set; }

        public string? BCC { get; set; }

        public string Subject { get; set; } = default!;

        public string Body { get; set; } = default!;
    }

    public class CreateEmailTemplateRequestValidator : CustomValidator<CreateEmailTemplateRequest>
    {
        public CreateEmailTemplateRequestValidator(IEmailTemplateRepository emailTemplateRepository)
        {
            RuleFor(p => p.TemplateCode)
           .NotEmpty()
               .WithMessage(DbRes.T("EmailTemplateCodeRequiredMsg"))
            .MustAsync(async (_, templateCode, _) => !await emailTemplateRepository.CheckEmailTemplateExist(templateCode))
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

    public class CreateEmailTemplateRequestHandler(IEmailTemplateRepository emailTemplateRepository) : IRequestHandler<CreateEmailTemplateRequest, Result<EmailTemplateDto>>
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository = emailTemplateRepository;

        public async Task<Result<EmailTemplateDto>> Handle(CreateEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var data = await _emailTemplateRepository.AddAsync(request.Adapt<EmailTemplate>());
            return Result.Succeed(data.Adapt<EmailTemplateDto>());
        }
    }
}
