using Academy.Application.Persistence.Repository;
using Academy.Application.Sport.Dto;
using Academy.Application.Sport.Query.Models;
using Academy.Application.Sport.Specifications;
using Academy.Domain.Entities;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Models;
using Mapster;

namespace Academy.Application.Sport.Query.Handlers
{
    public class GetSportListRequestHandler : IRequestHandler<GetSportListRequest, Result<PaginationResponse<SportsDto>>>
    {
        private readonly IReadRepository<Sports> _repository;
        public GetSportListRequestHandler(IReadRepository<Sports> repository)
        {
            _repository = repository;
        }

        public async Task<Result<PaginationResponse<SportsDto>>> Handle(GetSportListRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetSportListSpec(request);
            var data = await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);

            if (data.Data != null && data.Data.Count > 0)
            {
                var SportList = data.Data.Adapt<List<SportsDto>>();
                data.Data = SportList;
            }

            return Result.Succeed(data);
        }
    }
}
