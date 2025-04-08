using Academy.Application.UserPaymentInfo.Dto;
using Academy.Application.UserPaymentInfo.Query.Models;
using Academy.Shared.Pagination.Entity;
using Ardalis.Specification;
using Entites = Academy.Domain.Entities;

namespace Academy.Application.UserPaymentInfo.Specifications
{
    public class GetUserPaymentInfoListSpec : EntitiesByPaginationFilterSpec<Entites.UserPaymentInfo, UserPaymentInfoDto>
    {
        public GetUserPaymentInfoListSpec(GetUserPaymentInfoListRequest request) : base(request) => Query.OrderByDescending(x => x.CreatedOn);
    }
}
