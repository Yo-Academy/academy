using Academy.Application.CommonLookups;
using Academy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Application.Contracts.Persistence
{
    public interface ICommonLookupRepository : IScopedService, IRepository<CommonLookup>
    {
        IQueryable<CommonLookup> GetList();
        Task<CommonLookup> GetDetailsByIdAsync(Guid id);
        Task<bool> CheckLookupExist(string category, string key, Guid? id = default);
    }
}
