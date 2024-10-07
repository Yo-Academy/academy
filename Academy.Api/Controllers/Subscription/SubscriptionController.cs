using Academy.API.Controllers;
using Academy.Application.Subscription.Command.Models;
using Academy.Application.Subscription.Query.Models;

namespace Academy.Api.Controllers.Subscription
{
    public class SubscriptionController : VersionedApiController
    {
        private readonly IMediator _mediator;
        public SubscriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [OpenApiOperation("Gets a list of subscription.", "")]
        public async Task<ActionResult> GetListAsync()
        {
            return Ok(await _mediator.Send(new GetSubscriptionListRequest()));
        }

        [HttpPost]
        [OpenApiOperation("Creates a subscription.", "")]
        public async Task<ActionResult> CreateAsync(CreateSubscriptionRequest createBatchCommand)
        {
            return Ok(await _mediator.Send(createBatchCommand));
        }

        [HttpDelete("{id}")]
        [OpenApiOperation("Deletes subscription.", "")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteSubscriptionRequest { Id = id }));
        }

        [HttpGet("{id}")]
        [OpenApiOperation("Gets batch subscription based on id", "")]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            return Ok(await _mediator.Send(new GetSubscriptionDetailsRequest { Id = id }));
        }

        [HttpPut]
        [OpenApiOperation("Updates subscription details.", "")]
        public async Task<ActionResult> UpdateAsync(UpdateSubscriptionRequest updateBatchCommand)
        {
            return Ok(await _mediator.Send(updateBatchCommand));
        }
    }
}
