using Academy.API.Controllers;
using Academy.Application.Dashboard.Query.Models;

namespace Academy.Api.Controllers.Dashboard
{
    [Authorize]
    public class DashboardController : VersionedApiController
    {
        private readonly IMediator _mediator;
        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [OpenApiOperation("Gets dashboard details based on id", "")]
        public async Task<ActionResult> GetDashboardDetailsAsync(Guid id)
        {
            return Ok(await _mediator.Send(new GetDashboardListRequest { Id = id }));
        }
    }
}
