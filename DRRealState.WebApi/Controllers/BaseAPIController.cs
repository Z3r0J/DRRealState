using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DRRealState.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public abstract class BaseAPIController : ControllerBase
    {
    }
}
