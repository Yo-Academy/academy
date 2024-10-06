using Academy.Application.UserInfo.Dto;
using Academy.Application.UserInfo.Query.Models;
using Academy.Shared.Pagination.Entity;
using Ardalis.Specification;
using Entites = Academy.Domain.Entities;


namespace Academy.Application.UserInfo.Specifications
{
    public class GetUserInfoListSpec : EntitiesByPaginationFilterSpec<Entites.UserInfo, UserInfoDto>
    {
        public GetUserInfoListSpec(GetUserInfoListRequest request) : base(request) => Query.OrderByDescending(x => x.CreatedOn);
    }
}
