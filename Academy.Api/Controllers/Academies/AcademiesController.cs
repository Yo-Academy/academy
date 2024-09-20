using Academy.API.Controllers;
using Academy.Application.Academies.Command.Models;
using Academy.Application.Academies.Dto;
using Academy.Application.Academies.Query.Models;
using Academy.Application.Identity.Users;
using Academy.Infrastructure.Multitenancy;

namespace Academy.Api.Controllers.Academies
{
    [Authorize]
    public class AcademiesController : VersionedApiController
    {
        private readonly IMediator _mediator;
        private readonly ITenantResolverService _tenantResolverService;

        public AcademiesController(IMediator mediator, ITenantResolverService tenantResolverService)
        {
            _mediator = mediator;
            _tenantResolverService = tenantResolverService;
        }

        [HttpGet]
        [OpenApiOperation("Gets a list of academies.", "")]
        public async Task<ActionResult> GetListAsync()
        {
            return Ok(await _mediator.Send(new GetAcademiesListRequest()));
        }

        [HttpPost]
        [OpenApiOperation("Creates an academy.", "")]
        public async Task<ActionResult> CreateAsync([FromForm] CreateAcademiesRequest createAcademyCommand)
        {
            Result<AcademyDetailsDto> createResult = await _mediator.Send(createAcademyCommand);
            return CreatedAtRoute(new { id = createResult.Value?.Academy.Id }, createResult);
        }

        [HttpPost("academy-user")]
        //[TenantIdHeader]
        [OpenApiOperation("Creates an academy user.", "")]
        public async Task<ActionResult> CreateAcademyUserByRoleAsync(CreateAcademyUserRequest createAcademyUserCommand)
        {
            createAcademyUserCommand.Origin = Request.Scheme;

            //await _tenantResolverService.SwitchTenantAsync(Request.HttpContext, createAcademyUserCommand.TenantId);

            Result<UserDetailsDto> createResult = await _mediator.Send(createAcademyUserCommand);

            //await _tenantResolverService.RevertToPreviousTenantAsync(Request.HttpContext);

            return CreatedAtRoute(new { id = createResult.Value?.Id }, createResult);
        }

        [HttpDelete("{id}")]
        [OpenApiOperation("Deletes academy.", "")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteAcademiesRequest { Id = id }));
        }

        [HttpGet("{id}")]
        [OpenApiOperation("Gets academy details based on id", "")]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            return Ok(await _mediator.Send(new GetAcademyDetailsRequest { Id = id }));
        }

        [HttpPut("{id}")]
        [OpenApiOperation("Updates academy details.", "")]
        public async Task<ActionResult> UpdateAsync(Guid id, [FromForm] UpdateAcademiesRequest updateAcademyCommand)
        {
            updateAcademyCommand.Id = id;
            return Ok(await _mediator.Send(updateAcademyCommand));
        }
    }
}
