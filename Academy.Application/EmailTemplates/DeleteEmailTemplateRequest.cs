using Academy.Application.Common.Exceptions;
using Academy.Application.Contracts.Persistence;

namespace Academy.Application.EmailTemplates
{
    public class DeleteEmailTemplateRequest : IRequest<Result<string>>
    {
        public DefaultIdType Id { get; set; }
    }

    public class DeleteEmailTemplateRequestHandler(IEmailTemplateRepository emailTemplateRepository) : IRequestHandler<DeleteEmailTemplateRequest, Result<string>>
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository = emailTemplateRepository;

        public async Task<Result<string>> Handle(DeleteEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var emailTemplate = await _emailTemplateRepository.GetByIdAsync(request.Id);
            if (emailTemplate != null)
            {
                await _emailTemplateRepository.DeleteAsync(emailTemplate);
                return Result.Succeed(DbRes.T("EmailTemplateDeletedSuccessfullyMsg"));
            }
            else
            {
                throw new CustomException(DbRes.T("EmailTemplateDoesNotExistMsg"));
            }
        }
    }
}
