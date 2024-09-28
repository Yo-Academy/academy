using Academy.Application.Coaching.Dto;
using Academy.Application.Coaching.Query.Models;
using Academy.Application.Coaching.Specifications;
using Academy.Application.Persistence.Repository;
using Academy.Application.PlanType.Dto;
using Academy.Application.PlanType.Query.Models;
using Academy.Application.PlanType.Specifications;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = Academy.Domain.Entities.PlanType;


namespace Academy.Application.PlanType.Query.Handlers
{
    public class GetPlanTypeListRequestHandler : IRequestHandler<GetPlanTypeListRequest, Result<PaginationResponse<PlanTypeDto>>>
    {
        private readonly IReadRepository<Entity> _repository;
        public GetPlanTypeListRequestHandler(IReadRepository<Entity> repository)
        {
            _repository = repository;
        }
        public async Task<Result<PaginationResponse<PlanTypeDto>>> Handle(GetPlanTypeListRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetPlanTypeListSpec(request);
            var data = await _repository.PaginatedListAsync(spec,
                                                            request.PageNumber,
                                                            request.PageSize,
                                                            cancellationToken);

            if (data.Data != null && data.Data.Count > 0)
            {
                var planTypeList = data.Data.Adapt<List<PlanTypeDto>>();
                data.Data = planTypeList;
            }
            return Result.Succeed(data);
        }
    }
}
