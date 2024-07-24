using Academy.Application.EmailHistory;

namespace Academy.API.Controllers.EmailHistory
{
    [Authorize]
    public class EmailHistoryController : VersionedApiController
    {
        private readonly IMediator _mediator;
        public EmailHistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [OpenApiOperation("Gets a list of email history", "")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> GetListAsync()
        {
            return Ok(await _mediator.Send(new GetEmailHistoryListRequest()));
        }

        [HttpGet("{id}")]
        [OpenApiOperation("Gets email history details based on id", "")]
        public async Task<ActionResult> GetEmailHistoryDetailsAsync(Guid id)
        {
            return Ok(await _mediator.Send(new GetEmailHistoryDetailsRequest { Id = id }));
        }
    }
}
