using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DRRealState.Core.Application.DTOS.Account;
using DRRealState.Core.Application.Interfaces.Services;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace DRRealState.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [SwaggerTag("Authentication System")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _accountServices;

        public AccountController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        [HttpPost("authenticate")]
        [SwaggerOperation(
            Summary ="Authenticate",
            Description ="Authenticate to the API and get a JWT Token")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequest request) {


            return Ok(await _accountServices.AuthenticateAsync(request));
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost("register-administrator")]
        [SwaggerOperation(
            Summary = "Register an Administrator",
            Description = "Register an Administrator to use the endpoints included on the API (Only use by the an Administrator)")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> RegisterAdministratorAsync([FromBody]RegisterRequest request) {

            return Ok(await _accountServices.RegisterAdministratorAsync(request));
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost("register-developer")]
        [SwaggerOperation(
            Summary = "Register a Developer",
            Description = "Register a Developer to use the endpoints included on the API (Only use by the an Administrator)")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> RegisterDeveloperAsync([FromBody]RegisterRequest request) {

            return Ok(await _accountServices.RegisterDeveloperAsync(request));
        }

    }
}
