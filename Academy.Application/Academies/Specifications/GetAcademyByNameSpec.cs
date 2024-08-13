using Academy.Domain.Entities;
using Ardalis.Specification;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Entites = Academy.Domain.Entities;

namespace Academy.Application.Academies.Specifications
{
    public class GetAcademyByNameSpec : Specification<Entites.Academies>, ISingleResultSpecification<Entites.Academies>
    {
        public GetAcademyByNameSpec(string name, Guid? id = default)
        {
            Query.Where(x => x.Name.ToLower() == name.ToLower() && (!id.HasValue || id != x.Id));
        }
    }
}
