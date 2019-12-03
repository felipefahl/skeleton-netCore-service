using Microsoft.AspNetCore.Mvc;

namespace Skeleton.ServiceName.API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class ApiBaseController : ControllerBase
    {

    }
}
