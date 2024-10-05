using Academy.Application.Common.Exceptions;
using Academy.Application.Dashboard.Dto;
using Academy.Application.Dashboard.Query.Models;
using Academy.Application.Dashboard.Specifications;
using Academy.Application.Persistence.Repository;
using Academy.Shared;
using static System.Net.WebRequestMethods;
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
            List<DashboardDto> DashboardList = new List<DashboardDto>();
            var spec = new GetDashboardListSpec(request.Id);

            var academyDetails = await _repository.SingleOrDefaultAsync(spec, cancellationToken);
            if (academyDetails == null || academyDetails.AcademySportsMappings.Count == 0)
            {
                return Result.Fail(new NotFoundException(DbRes.T("ErrorMessageAcademyNotFound")));
            }

            if (academyDetails != null && academyDetails.AcademySportsMappings.Count > 0)
            {
                foreach (var item in academyDetails.AcademySportsMappings)
                {
                    if (item != null && item.Sports != null)
                    {
                        DashboardList.Add(new DashboardDto
                        {
                            Id = item.Sports.Id,
                            Name = item.Sports.Name,
                            ActiveAcademyPlayers = 0,
                            ActiveMembers = 2,
                            ActiveUsers = 3,
                            Image = !string.IsNullOrEmpty(item.Sports.Image) ? string.Format(Constants.CloudFrontUrl, Constants.S3Directory.Sports, item.Sports.Image) : string.Empty, // Not able to generate cloud front url using mapping so as of not put static link for sports image.
                            Users = 2
                        });
                    }
                }
            }

            return Result.Succeed(DashboardList);
        }
    }
}
