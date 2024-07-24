using Academy.Application.Contracts.Persistence;
using Academy.Domain.Entities;

namespace Academy.Application.EmailTemplates
{
    public class GetEmailTemplateListRequest : IRequest<Result<List<EmailTemplate>>>;

    public class GetEmailTemplateListRequestHandler(IEmailTemplateRepository emailTemplateRepository) : IRequestHandler<GetEmailTemplateListRequest, Result<List<EmailTemplate>>>
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository = emailTemplateRepository;

        public async Task<Result<List<EmailTemplate>>> Handle(GetEmailTemplateListRequest request, CancellationToken cancellationToken) =>
             Result.Succeed(_emailTemplateRepository.GetList().ToList());
    }
}
