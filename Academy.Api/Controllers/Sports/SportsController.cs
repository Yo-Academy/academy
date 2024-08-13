using Academy.API.Controllers;
using Academy.Application.Academies.Command.Models;
using Academy.Application.Academies.Query.Models;
using Academy.Application.Sport.Command.Models;
using Academy.Application.Sport.Query.Models;

namespace Academy.Api.Controllers.Sports
{
    [Authorize]
    public class SportsController : VersionedApiController
    {
        private readonly IMediator _mediator;
        public SportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [OpenApiOperation("Gets a list of Sports.", "")]
        public async Task<ActionResult> GetListAsync()
        {
            return Ok(await _mediator.Send(new GetSportListRequest()));
        }

        [HttpPost]
        [OpenApiOperation("Creates an Sport.", "")]
        public async Task<ActionResult> CreateAsync(CreateSportRequest createSportCommand)
        {
            return Ok(await _mediator.Send(createSportCommand));
        }

        [HttpDelete("{id}")]
        [OpenApiOperation("Deletes Sport.", "")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteSportRequest { Id = id }));
        }

        [HttpGet("{id}")]
        [OpenApiOperation("Gets sport details based on id", "")]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            return Ok(await _mediator.Send(new GetSportDetailsRequest { Id = id }));
        }

        [HttpPut("{id}")]
        [OpenApiOperation("Updates academy details.", "")]
        public async Task<ActionResult> UpdateAsync(Guid id, UpdateSportRequest updateSportCommand)
        {
            updateSportCommand.Id = id;
            return Ok(await _mediator.Send(updateSportCommand));
        }
    }
}
