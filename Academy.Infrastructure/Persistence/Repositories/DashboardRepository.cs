using Academy.Application.Contracts.Persistence;
using Academy.Application.Dashboard.Dto;
using Academy.Application.Dashboard.Query.Models;
using Academy.Domain.Entities;
using Academy.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Academy.Infrastructure.Persistence.Repositories
{
    public class DashboardRepository : Repository<DashboardDto>, IDashboardRepository
    {
        protected readonly ICurrentUser _currentUser;
        public DashboardRepository(ApplicationDbContext dbContext,
            ICurrentUser currentUser) : base(dbContext)
        {
            _currentUser = currentUser;
        }

        public IQueryable<DashboardDto> GetList(GetDashboardListRequest request)
        {
            //IQueryable<DashboardDto> dashboards1 = null;
            //var academy = _dbContext.Academies.Where(x => x.Id == request.Id).FirstOrDefault();
            //if (academy != null)
            //{
            //    var mappings = _dbContext.AcademySportsMappings.Where(x => x.AcademyId == academy.Id).ToList();
            //    if (mappings.Count > 0)
            //    {
            //        var sports = _dbContext.Sports.Where(x => mappings.Select(y => y.SportId).Contains(x.Id));
                    
            //        IQueryable<DashboardDto> dashboards = from a in sports
            //                                              select new DashboardDto
            //                                              {
            //                                                  Id = a.Id,
            //                                                  Name = a.Name,
            //                                                  ActiveAcademyPlayers = 0,
            //                                                  ActiveMembers = 0,
            //                                                  ActiveUsers = 0,
            //                                                  Image = a.Image,
            //                                                  Users = 0
            //                                              };
            //        return dashboards;
            //    }
            //}


            IQueryable<DashboardDto> dashboards = from a in _dbContext.Academies
                                                  join m in _dbContext.AcademySportsMappings on a.Id equals m.AcademyId
                                                  join s in _dbContext.Sports on m.SportId equals s.Id
                                                  //where a.Id == request.Id
                                                  select new DashboardDto
                                                  {
                                                      Id = s.Id,
                                                      Name = s.Name,
                                                      ActiveAcademyPlayers = 0,
                                                      ActiveMembers = 0,
                                                      ActiveUsers = 0,
                                                      Image = s.Image,
                                                      Users = 0
                                                  };


            //var query = _dbContext.Academies
            //.Include(x => x.AcademySportsMappings)
            //.Where(x => x.Id == request.Id)
            //.Select(x =>
            //new DashboardDto
            //{
            //    Id = x.Id,

            //});

            return dashboards;
        }
    }
}
