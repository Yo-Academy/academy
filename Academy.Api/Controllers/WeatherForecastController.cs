using Academy.Application.Features.Mailing;
using Academy.Application.Features.Storage;
using Academy.Application.Features.WeatherForecasts.Queries;

namespace Academy.API.Controllers
{
    public class WeatherForecastController : VersionedApiController
    {

        private readonly IMediator _mediator;

        public WeatherForecastController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
        {
            var dtos = await _mediator.Send(new GetWeatherForecastsQuery());
            return Ok(dtos);
        }

        [HttpPost]
        [Route("upload-file")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<bool> UploadFile(UploadFileRequest uploadFileCommand)
        {
            var res = await _mediator.Send(uploadFileCommand);
            return res;
        }

        [HttpPost]
        [Route("send-mail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<bool> SendMail(MailSendRequest mailCommand)
        {
            var res = await _mediator.Send(mailCommand);
            return res;
        }

        //hangfire testing method
        [HttpPost("generate-random")]
        [OpenApiOperation("Generate a number of random strings.", "")]
        [Authorize]
        public Task<string> GenerateRandomAsync(GenerateRandomBrandRequest request)
        {
            return Mediator.Send(request);
        }
    }
}
