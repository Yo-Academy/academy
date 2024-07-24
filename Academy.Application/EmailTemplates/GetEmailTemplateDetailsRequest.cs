using Academy.Application.Contracts.Persistence;
using Mapster;

namespace Academy.Application.EmailTemplates
{
    public class GetEmailTemplateDetailsRequest : IRequest<Result<EmailTemplateDto>>
    {
        public DefaultIdType Id { get; set; }
    }

    public class GetEmailTemplateDetailsRequestHandler(IEmailTemplateRepository emailTemplateRepository) : IRequestHandler<GetEmailTemplateDetailsRequest, Result<EmailTemplateDto>>
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository = emailTemplateRepository;

        public async Task<Result<EmailTemplateDto>> Handle(GetEmailTemplateDetailsRequest request, CancellationToken cancellationToken)
        {
            return Result.Succeed((await _emailTemplateRepository.GetByIdAsync(request.Id)).Adapt<EmailTemplateDto>());
        }

    }
}
