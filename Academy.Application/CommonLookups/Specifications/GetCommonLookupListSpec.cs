using Academy.Application.CommonLookups.Dto;
using Academy.Application.CommonLookups.Query.Models;
using Academy.Domain.Entities;
using Academy.Shared.Pagination;
using Academy.Shared.Pagination.Entity;
using Academy.Shared.Pagination.Models;
using Ardalis.Specification;

namespace Academy.Application.CommonLookups.Specifications;

public class GetCommonLookupListSpec : EntitiesByPaginationFilterSpec<CommonLookup, CommonLookupsDto>
{
    public GetCommonLookupListSpec(GetCommonLookupListRequest request)
      : base(request) =>
      Query.OrderByDescending(x => x.CreatedOn, !request.HasOrderBy());
}
