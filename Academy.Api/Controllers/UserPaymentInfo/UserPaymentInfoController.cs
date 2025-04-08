using Academy.API.Controllers;
using Academy.Application.UserPaymentInfo.Command.Models;
using Academy.Application.UserPaymentInfo.Query.Models;

namespace Academy.Api.Controllers.UserPaymentInfo
{
    public class UserPaymentInfoController : VersionedApiController
    {
        private readonly IMediator _mediator;
        public UserPaymentInfoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [OpenApiOperation("Gets a list of User payment info.", "")]
        public async Task<ActionResult> GetListAsync()
        {
            return Ok(await _mediator.Send(new GetUserPaymentInfoListRequest()));
        }

        [HttpPost]
        [OpenApiOperation("Creates an User payment info.", "")]
        public async Task<ActionResult> CreateAsync(CreateUserPaymentInfoRequest createUserPaymentInfoCommand)
        {
            return Ok(await _mediator.Send(createUserPaymentInfoCommand));
        }

        [HttpDelete("{id}")]
        [OpenApiOperation("Deletes User payment info.", "")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteUserPaymentInfoRequest { Id = id }));
        }

        [HttpGet("{id}")]
        [OpenApiOperation("Gets user payment info based on id", "")]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            return Ok(await _mediator.Send(new GetUserPaymentInfoDetailRequest { Id = id }));
        }

        [HttpPut]
        [OpenApiOperation("Updates user payment info details.", "")]
        public async Task<ActionResult> UpdateAsync(UpdateUserPaymentInfoRequest updateUserPaymentInfoCommand)
        {
            return Ok(await _mediator.Send(updateUserPaymentInfoCommand));
        }
    }
}
