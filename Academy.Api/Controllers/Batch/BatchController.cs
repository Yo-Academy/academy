using Academy.API.Controllers;
using Academy.Application.Batch.Command.Models;
using Academy.Application.Batch.Query.Models;

namespace Academy.Api.Controllers.Batch
{
    public class BatchController : VersionedApiController
    {
        private readonly IMediator _mediator;
        public BatchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [OpenApiOperation("Gets a list of batches.", "")]
        public async Task<ActionResult> GetListAsync()
        {
            return Ok(await _mediator.Send(new GetBatchListRequest()));
        }

        [HttpPost]
        [OpenApiOperation("Creates an batch.", "")]
        public async Task<ActionResult> CreateAsync(CreateBatchRequest createBatchCommand)
            {
            return Ok(await _mediator.Send(createBatchCommand));
        }

        [HttpDelete("{id}")]
        [OpenApiOperation("Deletes batch.", "")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteBatchRequest { Id = id }));
        }

        [HttpGet("{id}")]
        [OpenApiOperation("Gets batch details based on id", "")]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            return Ok(await _mediator.Send(new GetBatchDetailsRequest { Id = id }));
        }

        [HttpPut("{id}")]
        [OpenApiOperation("Updates Batch details.", "")]
        public async Task<ActionResult> UpdateAsync(Guid id, UpdateBatchRequest updateBatchCommand)
        {
            updateBatchCommand.Id = id;
            return Ok(await _mediator.Send(updateBatchCommand));
        }
    }
}
