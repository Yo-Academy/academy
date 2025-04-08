using Academy.API.Controllers;
using Academy.Application.PaymentType.Command.Models;
using Academy.Application.PaymentType.Query.Models;

namespace Academy.Api.Controllers.PaymentType
{
    public class PaymentTypeController : VersionedApiController
    {
        private readonly IMediator _mediator;
        public PaymentTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [OpenApiOperation("Creates a payment type.", "")]
        public async Task<ActionResult> CreateAsync(CreatePaymentTypeRequest createPaymentTypeCommand)
        {
            return Ok(await _mediator.Send(createPaymentTypeCommand));
        }

        [HttpGet]
        [OpenApiOperation("Gets a list of payment type.", "")]
        public async Task<ActionResult> GetListAsync()
        {
            return Ok(await _mediator.Send(new GetPaymentTypeListRequest()));
        }
    }
}
