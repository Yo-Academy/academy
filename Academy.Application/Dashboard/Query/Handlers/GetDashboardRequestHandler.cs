using Academy.Application.Dashboard.Dto;
using Academy.Application.Dashboard.Query.Models;
using Academy.Application.Dashboard.Specifications;
using Academy.Application.Persistence.Repository;
using Entity = Academy.Domain.Entities;

namespace Academy.Application.Dashboard.Query.Handlers
{
    public class GetDashboardListRequestHandler : IRequestHandler<GetDashboardListRequest, Result<List<DashboardDto>>>
    {
        private readonly IReadRepository<Entity.Academies> _repository;
        public GetDashboardListRequestHandler(IReadRepository<Entity.Academies> repository)
        {
            _repository = repository;
        }
        public async Task<Result<List<DashboardDto>>> Handle(GetDashboardListRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetDashboardListSpec(request);

            var data = await _repository.ListAsync(spec, cancellationToken);

            List<DashboardDto> DashboardList = new List<DashboardDto> {
                new DashboardDto
                {
                    Id = Guid.Parse("5bacf17d-a35a-4a31-93e0-75555387e1de"),
                    Name = "Cricket",
                    ActiveAcademyPlayers = 0,
                    ActiveMembers = 2,
                    ActiveUsers = 3,
                    Image = "",
                    Users = 2
                },
                new DashboardDto
                {
                    Id = Guid.Parse("3f7d0aae-4f9a-436e-b7c5-185568b917a0"),
                    Name = "Football",
                    ActiveAcademyPlayers = 0,
                    ActiveMembers = 2,
                    ActiveUsers = 3,
                    Image = "",
                    Users = 2
                },
                new DashboardDto
                {
                    Id = Guid.Parse("4f0e7e00-ec17-4595-8b54-6c8c618cd276"),
                    Name = "Tennis",
                    ActiveAcademyPlayers = 0,
                    ActiveMembers = 2,
                    ActiveUsers = 3,
                    Image = "",
                    Users = 2
                },
                new DashboardDto
                {
                    Id = Guid.Parse("a681e399-50a9-48a9-90b9-e359f893566d"),
                    Name = "Badminton",
                    ActiveAcademyPlayers = 0,
                    ActiveMembers = 2,
                    ActiveUsers = 3,
                    Image = "",
                    Users = 2
                }
             };

            //if (data != null && data.Count > 0)
            //{
            //    DashboardList.AddRange(data.Select(x => new DashboardDto
            //    {
            //        Id = x.Id,
            //        Name = x.Name,
            //        ActiveAcademyPlayers = 0,
            //        ActiveMembers = 2,
            //        ActiveUsers = 3,
            //        Image = "",
            //        Users = 2
            //    }).ToList());
            //    return Result.Succeed(DashboardList);
            //}
            return Result.Succeed(DashboardList);
        }
    }
}
