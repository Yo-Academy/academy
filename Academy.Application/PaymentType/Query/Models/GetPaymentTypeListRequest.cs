using Academy.Application.PaymentType.Dto;
using Academy.Shared.Pagination.Models;

namespace Academy.Application.PaymentType.Query.Models
{
    public class GetPaymentTypeListRequest : PaginationFilter, IRequest<Result<PaginationResponse<PaymentTypeDto>>>
    {
    }
}
