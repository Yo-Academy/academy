using Academy.Application.Dashboard.Dto;

namespace Academy.Application.Dashboard.Query.Models
{
    public class GetDashboardListRequest : IRequest<Result<List<DashboardDto>>>
    {
        public DefaultIdType Id { get; set; }
    }
}
