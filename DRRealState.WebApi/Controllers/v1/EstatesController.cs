using DRRealState.Core.Application.DTOS.Estates;
using DRRealState.Core.Application.Features.Estates.Queries.GetAllEstatesQuery;
using DRRealState.Core.Application.Features.Estates.Queries.GetEstatesByCodeQuery;
using DRRealState.Core.Application.Features.Estates.Queries.GetEstatesByIdQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DRRealState.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    public class EstatesController : BaseAPIController
    {
        [Authorize(Roles = "ADMINISTRATOR,DEVELOPER")]
        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EstatesResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get() {

            try
            {

                return Ok(await Mediator.Send(new GetAllEstatesQuery()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        
        }
        [Authorize(Roles="ADMINISTRATOR,DEVELOPER")]
        [HttpGet("GetByCode/{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EstatesResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByCode(string code) {

            try
            {

                return Ok(await Mediator.Send(new GetEstatesByCodeQuery() { Code = code }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        
        }
        [Authorize(Roles = "ADMINISTRATOR,DEVELOPER")]
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EstatesResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id) {

            try
            {

                return Ok(await Mediator.Send(new GetEstatesByIdQuery() { Id = id }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        
        }
    }
}
