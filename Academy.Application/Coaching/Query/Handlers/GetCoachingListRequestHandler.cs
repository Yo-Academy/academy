using Academy.Application.Coaching.Dto;
using Academy.Application.Coaching.Query.Models;
using Academy.Application.Coaching.Specifications;
using Academy.Application.Persistence.Repository;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Models;
using Mapster;
using Entity = Academy.Domain.Entities.Coaching;

namespace Academy.Application.Coaching.Query.Handlers
{
    public class GetCoachingListRequestHandler : IRequestHandler<GetCoachingListRequest, Result<PaginationResponse<CoachingDto>>>
    {
        private readonly IReadRepository<Entity> _repository;
        public GetCoachingListRequestHandler(IReadRepository<Entity> repository)
        {
            _repository = repository;
        }

        public async Task<Result<PaginationResponse<CoachingDto>>> Handle(GetCoachingListRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetCoachingListSpec(request);
            var data = await _repository.PaginatedListAsync(spec,
                                                            request.PageNumber,
                                                            request.PageSize,
                                                            cancellationToken);

            if (data.Data != null && data.Data.Count > 0)
            {
                var coachingList = data.Data.Adapt<List<CoachingDto>>();
                data.Data = coachingList;
            }
            return Result.Succeed(data);
        }
    }
}
