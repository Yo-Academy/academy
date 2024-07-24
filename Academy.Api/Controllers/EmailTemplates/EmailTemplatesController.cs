using Academy.Application.EmailTemplates;

namespace Academy.API.Controllers.EmailTemplates
{
    [Authorize]
    public class EmailTemplatesController : VersionedApiController
    {
        private readonly IMediator _mediator;
        public EmailTemplatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [OpenApiOperation("Gets a list of email templates.", "")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> GetListAsync()
        {
            return Ok(await _mediator.Send(new GetEmailTemplateListRequest()));
        }

        [HttpPost]
        [OpenApiOperation("Creates an email template.", "")]
        public async Task<ActionResult> CreateAsync(CreateEmailTemplateRequest createEmailTemplateCommand)
        {
            return Ok(await _mediator.Send(createEmailTemplateCommand));
        }

        [HttpDelete("{id}")]
        [OpenApiOperation("Deletes an email template.", "")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteEmailTemplateRequest { Id = id }));
        }

        [HttpGet("{id}")]
        [OpenApiOperation("Gets details of email template based on id", "")]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            return Ok(await _mediator.Send(new GetEmailTemplateDetailsRequest { Id = id }));
        }

        [HttpPut("{id}")]
        [OpenApiOperation("Updates an email template.", "")]
        public async Task<ActionResult> UpdateAsync(Guid id, UpdateEmailTemplateRequest updateEmailTemplateCommand)
        {
            updateEmailTemplateCommand.Id = id;
            return Ok(await _mediator.Send(updateEmailTemplateCommand));
        }
    }
}
