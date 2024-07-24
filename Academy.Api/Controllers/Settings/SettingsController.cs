using Academy.Application.Settings;

namespace Academy.API.Controllers.Settings
{
    [Authorize]
    public class SettingsController : VersionedApiController
    {
        private readonly IMediator _mediator;
        public SettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [OpenApiOperation("Gets a list of settings.", "")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> GetListAsync()
        {
            return Ok(await _mediator.Send(new GetSettingListRequest()));
        }

        [HttpPost]
        [OpenApiOperation("Creates a setting.", "")]
        public async Task<ActionResult> CreateAsync(CreateSettingRequest createSettingCommand)
        {
            return Ok(await _mediator.Send(createSettingCommand));
        }

        [HttpDelete("{id}")]
        [OpenApiOperation("Deletes a setting.", "")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteSettingRequest { Id = id }));
        }

        [HttpGet("{id}")]
        [OpenApiOperation("Gets details of setting based on id", "")]
        [ResponseCache(Duration = 300, VaryByQueryKeys = ["id"])]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            return Ok(await _mediator.Send(new GetSettingDetailsRequest { Id = id }));
        }

        [HttpPut("{id}")]
        [OpenApiOperation("Updates a setting.", "")]
        public async Task<ActionResult> UpdateAsync(Guid id, UpdateSettingRequest updateSettingCommand)
        {
            updateSettingCommand.Id = id;
            return Ok(await _mediator.Send(updateSettingCommand));
        }
    }
}
