using Academy.API.Controllers;
using Academy.Application.Academies.Command.Models;
using Academy.Application.Academies.Query.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Api.Controllers.Academies
{
    [Authorize]
    public class AcademiesController : VersionedApiController
    {
        private readonly IMediator _mediator;
        public AcademiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [OpenApiOperation("Gets a list of academies.", "")]
        public async Task<ActionResult> GetListAsync()
        {
            return Ok(await _mediator.Send(new GetAcademiesListRequest()));
        }

        [HttpPost]
        [OpenApiOperation("Creates an academy.", "")]
        public async Task<ActionResult> CreateAsync(CreateAcademiesRequest createAcademyCommand)
        {
            return Ok(await _mediator.Send(createAcademyCommand));
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
        public async Task<ActionResult> UpdateAsync(Guid id, UpdateAcademiesRequest updateAcademyCommand)
        {
            updateAcademyCommand.Id = id;
            return Ok(await _mediator.Send(updateAcademyCommand));
        }
    }
}
