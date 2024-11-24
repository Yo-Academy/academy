using Academy.Application.PaymentType.Dto;
using Academy.Application.PaymentType.Query.Models;
using Academy.Application.PlanType.Query.Models;
using Academy.Shared.Pagination.Entity;
using Ardalis.Specification;
using Entity = Academy.Domain.Entities;

namespace Academy.Application.PaymentType.Specifications
{
    public class GetPaymentTypeListSpec : EntitiesByPaginationFilterSpec<Entity.PaymentType, PaymentTypeDto>
    {
        public GetPaymentTypeListSpec(GetPaymentTypeListRequest request) : base(request) => Query.OrderByDescending(x => x.CreatedOn);
       
    }
}
