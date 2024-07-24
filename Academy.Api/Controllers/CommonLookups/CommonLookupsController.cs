using Academy.Application.CommonLookups;
using Academy.Application.CommonLookups.Command.Models;
using Academy.Application.CommonLookups.Query.Models;

namespace Academy.API.Controllers.CommonLookups
{
    [Authorize]
    public class CommonLookupsController : VersionedApiController
    {
        private readonly IMediator _mediator;
        public CommonLookupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [OpenApiOperation("Gets a list of common lookups.", "")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> GetListAsync()
        {
            return Ok(await _mediator.Send(new GetCommonLookupListRequest()));
        }

        [HttpGet("{id}")]
        [OpenApiOperation("Gets details of common lookup based on id", "")]
        [ResponseCache(Duration = 300, VaryByQueryKeys = ["id"])]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            return Ok(await _mediator.Send(new GetCommonLookupRequest { Id = id }));
        }

        [HttpDelete("{id}")]
        [OpenApiOperation("Deletes a common lookup.", "")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteCommonLookupRequest { Id = id }));
        }

        [HttpPost]
        [OpenApiOperation("Creates a common lookup.", "")]
        public async Task<ActionResult> CreateAsync(CreateCommonLookupRequest createCommonLookupRequest)
        {
            return Ok(await _mediator.Send(createCommonLookupRequest));
        }

        [HttpPut("{id}")]
        [OpenApiOperation("Updates a common lookup.", "")]
        public async Task<ActionResult> UpdateAsync(Guid id, UpdateCommonLookupRequest updateCommonLookupCommand)
        {
            updateCommonLookupCommand.Id = id;
            return Ok(await _mediator.Send(updateCommonLookupCommand));
        }
    }
}
