using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entites = Academy.Domain.Entities;


namespace Academy.Application.Academies.Specifications
{
    public class GetAcademyByIdSpec : Specification<Entites.Academies>, ISingleResultSpecification<Entites.Academies>
    {
        public GetAcademyByIdSpec(DefaultIdType id)
        {
            Query.Where(x => x.Id == id).Include(x => x.AcademySportsMappings);
        }
    }
}
