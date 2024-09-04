using Academy.API.Controllers;
using Academy.Application.Coaching.Command.Models;

namespace Academy.Api.Controllers.Coaching
{
    public class CoachingController : VersionedApiController
    {
        private readonly IMediator _mediator;
        public CoachingController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [OpenApiOperation("Creates an coaching.", "")]
        public async Task<ActionResult> CreateAsync(CreateCoachingRequest createBatchCommand)
        {
            return Ok(await _mediator.Send(createBatchCommand));
        }
    }
}
