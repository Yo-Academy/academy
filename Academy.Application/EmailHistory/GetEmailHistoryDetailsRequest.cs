using Academy.Application.Common.Exceptions;
using Academy.Application.Contracts.Persistence;
using Mapster;
using System.Text;

namespace Academy.Application.EmailHistory
{
    public class GetEmailHistoryDetailsRequest : IRequest<Result<EmailHistoryDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetEmailHistoryDetailsRequestHandler : IRequestHandler<GetEmailHistoryDetailsRequest, Result<EmailHistoryDto>>
    {
        private readonly IEmailHistoryRepository _emailHistoryRepository;

        public GetEmailHistoryDetailsRequestHandler(IEmailHistoryRepository emailHistoryRepository) =>
            _emailHistoryRepository = emailHistoryRepository;

        public async Task<Result<EmailHistoryDto>> Handle(GetEmailHistoryDetailsRequest request, CancellationToken cancellationToken)
        {
            var emailHistory = await _emailHistoryRepository.GetEmailHistoryByIdAsync(request.Id);
            _ = emailHistory ?? throw new NotFoundException(DbRes.T("EmailHistoryNotFoundMsg"));
            var emailHistoryDto = emailHistory.Adapt<EmailHistoryDto>();
            emailHistoryDto.Body = Encoding.UTF8.GetString(_emailHistoryRepository.Decompress(emailHistory.Body));
            return Result.Succeed(emailHistoryDto);
        }
    }
}
