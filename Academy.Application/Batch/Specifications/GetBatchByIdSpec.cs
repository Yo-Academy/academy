using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entites = Academy.Domain.Entities;


namespace Academy.Application.Batch.Specifications
{
    public class GetBatchByIdSpec : Specification<Entites.Batch>, ISingleResultSpecification<Entites.Batch>
    {
        public GetBatchByIdSpec(DefaultIdType id)
        {
            Query.Where(x => x.Id == id).Include(x => x.Sports).Include(x =>x.Coaching);
        }
    }
}
