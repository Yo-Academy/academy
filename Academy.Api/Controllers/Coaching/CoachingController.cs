using Academy.API.Controllers;
using Academy.Application.Coaching.Command.Models;
using Academy.Application.Coaching.Query.Models;
using Academy.Application.Sport.Query.Models;

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

        [HttpGet]
        [OpenApiOperation("Gets a list of coaching.", "")]
        public async Task<ActionResult> GetListAsync()
        {
            return Ok(await _mediator.Send(new GetCoachingListRequest()));
        }
    }
}
