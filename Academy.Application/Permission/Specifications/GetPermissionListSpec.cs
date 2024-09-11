using Ardalis.Specification;
using Academy.Domain.Entities;
using Academy.Application.Permission.Query.Models;
using Academy.Shared.Pagination.Entity;
using Academy.Application.Permission.Dto;
using Academy.Application.Academies.Query.Models;
using Azure.Core;

namespace Academy.Application.Permission.Specifications
{
    public class GetPermissionListSpec : EntitiesByPaginationFilterSpec<Permissions, PermissionDto>
    {
        public GetPermissionListSpec(GetPermissionListRequest request) : base(request)
        {
            Query.OrderByDescending(x => x.CreatedOn);
        }
    }
}
