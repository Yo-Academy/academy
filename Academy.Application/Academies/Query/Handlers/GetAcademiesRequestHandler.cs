using Academy.Application.Academies.Dto;
using Academy.Application.Academies.Query.Models;
using Academy.Application.Academies.Specifications;
using Academy.Application.Persistence.Repository;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Models;
using Mapster;
using Entity = Academy.Domain.Entities;

namespace Academy.Application.Academies.Query.Handlers
{
    public class GetAcademiesListRequestHandler : IRequestHandler<GetAcademiesListRequest, Result<PaginationResponse<AcademiesDto>>>
    {
        private readonly IReadRepository<Entity.Academies> _repository;
        public GetAcademiesListRequestHandler(IReadRepository<Entity.Academies> repository)
        {
            _repository = repository;
        }

        public async Task<Result<PaginationResponse<AcademiesDto>>> Handle(GetAcademiesListRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetAcademiesListSpec(request);
            var data = await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);


            if (data.Data != null && data.Data.Count > 0)
            {
                var academiesList = data.Data.Adapt<List<AcademiesDto>>();
                data.Data = academiesList;
            }
            return Result.Succeed(data);
        }
    }
}
