using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = Academy.Domain.Entities;


namespace Academy.Application.Contracts.Persistence
{
    public interface IBatchRepository : IScopedService, IRepository<Entity.Batch>
    {
       // IQueryable<Entity.Batch> GetBatchList();
    }
}
