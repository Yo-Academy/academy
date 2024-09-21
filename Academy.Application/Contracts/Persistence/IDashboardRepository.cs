using Academy.Application.Dashboard.Dto;
using Academy.Application.Dashboard.Query.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Application.Contracts.Persistence
{
    public interface IDashboardRepository : IScopedService, IRepository<DashboardDto>
    {
        IQueryable<DashboardDto> GetList(GetDashboardListRequest request);
    }
}
