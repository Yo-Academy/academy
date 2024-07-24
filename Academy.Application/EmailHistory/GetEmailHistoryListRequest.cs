using Academy.Application.Contracts.Persistence;
using Mapster;

namespace Academy.Application.EmailHistory
{
    public class GetEmailHistoryListRequest : IRequest<Result<List<EmailHistoryListDto>>>;

    public class GetEmailHistoryListRequestHandler(IEmailHistoryRepository emailHistoryRepository) : IRequestHandler<GetEmailHistoryListRequest, Result<List<EmailHistoryListDto>>>
    {
        private readonly IEmailHistoryRepository _emailHistoryRepository = emailHistoryRepository;

        public async Task<Result<List<EmailHistoryListDto>>> Handle(GetEmailHistoryListRequest request, CancellationToken cancellationToken) =>
           Result.Succeed(_emailHistoryRepository.GetList().Adapt<List<EmailHistoryListDto>>());
    }
}
