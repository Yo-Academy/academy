using Academy.Application.Persistence.Repository;
using Academy.Application.UserPaymentInfo.Dto;
using Academy.Application.UserPaymentInfo.Query.Models;
using Academy.Application.UserPaymentInfo.Specifications;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Models;
using Mapster;
using Entites = Academy.Domain.Entities;

namespace Academy.Application.UserPaymentInfo.Query.Handlers
{
    public class GetUserPaymentInfoListRequestHandler : IRequestHandler<GetUserPaymentInfoListRequest, Result<PaginationResponse<UserPaymentInfoDto>>>
    {
        private readonly IReadRepository<Entites.UserPaymentInfo> _repository;
        public GetUserPaymentInfoListRequestHandler(IReadRepository<Entites.UserPaymentInfo> repository)
        {
            _repository = repository;
        }
        public async Task<Result<PaginationResponse<UserPaymentInfoDto>>> Handle(GetUserPaymentInfoListRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetUserPaymentInfoListSpec(request);
            var data = await _repository.PaginatedListAsync(spec,
                                                           request.PageNumber,
                                                           request.PageSize,
                                                           cancellationToken);
            if (data != null)
            {
                var userPaymentInfoList = data.Data.Adapt<List<UserPaymentInfoDto>>();
                data.Data = userPaymentInfoList;

            }
            return Result.Succeed(data);
        }
    }
}
