using Academy.Application.Academies.Dto;
using Academy.Application.Academies.Query.Models;
using Academy.Domain.Entities;
using Academy.Shared.Pagination.Entity;
using Ardalis.Specification;

namespace Academy.Application.Academies.Specifications
{
    public class GetAcademySportsByAcademyIdSpec : Specification<AcademySportsMapping, AcademySportsMapping>
    {
        public GetAcademySportsByAcademyIdSpec(DefaultIdType id)
             => Query.Where(x => x.AcademyId == id).OrderByDescending(x => x.CreatedOn);
    }
}
