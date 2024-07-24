using Asp.Versioning;

namespace Academy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiVersionNeutral]
    public class VersionNeutralApiController : BaseApiController
    {
    }
}
