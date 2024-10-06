using Academy.API.Controllers;
using Academy.Application.UserInfo.Command.Models;
using Academy.Application.UserInfo.Query.Models;

namespace Academy.Api.Controllers.UserInfo
{
    public class UserInfoController : VersionedApiController
    {
        private readonly IMediator _mediator;
        public UserInfoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [OpenApiOperation("Gets a list of User.", "")]
        public async Task<ActionResult> GetListAsync()
        {
            return Ok(await _mediator.Send(new GetUserInfoListRequest()));
        }

        [HttpPost]
        [OpenApiOperation("Creates an User.", "")]
        public async Task<ActionResult> CreateAsync(CreateUserInfoRequest createUserInfoCommand)
        {
            return Ok(await _mediator.Send(createUserInfoCommand));
        }

        [HttpDelete("{id}")]
        [OpenApiOperation("Deletes User info.", "")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteUserInfoRequest { Id = id }));
        }

        [HttpGet("{id}")]
        [OpenApiOperation("Gets user info based on id", "")]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            return Ok(await _mediator.Send(new GetUserInfoDetailRequest { Id = id }));
        }

        [HttpPut]
        [OpenApiOperation("Updates user info details.", "")]
        public async Task<ActionResult> UpdateAsync(UpdateUserInfoRequest updateUserInfoCommand)
        {
            return Ok(await _mediator.Send(updateUserInfoCommand));
        }
    }
}
