using Academy.Application.PaymentType.Dto;
using Academy.Application.PaymentType.Query.Models;
using Academy.Application.PaymentType.Specifications;
using Academy.Application.Persistence.Repository;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Models;
using Mapster;
using Entity = Academy.Domain.Entities.PaymentType;

namespace Academy.Application.PaymentType.Query.Handlers
{
    public class GetPaymentTypeListRequestHandler : IRequestHandler<GetPaymentTypeListRequest, Result<PaginationResponse<PaymentTypeDto>>>
    {
        private readonly IReadRepository<Entity> _repository;
        public GetPaymentTypeListRequestHandler(IReadRepository<Entity> repository)
        {
            _repository = repository;
        }
        public async Task<Result<PaginationResponse<PaymentTypeDto>>> Handle(GetPaymentTypeListRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetPaymentTypeListSpec(request);
            var data = await _repository.PaginatedListAsync(spec,
                                                            request.PageNumber,
                                                            request.PageSize,
                                                            cancellationToken);

            if (data.Data != null && data.Data.Count > 0)
            {
                var paymentTypeList = data.Data.Adapt<List<PaymentTypeDto>>();
                data.Data = paymentTypeList;
            }
            return Result.Succeed(data);
        }
    }
}
