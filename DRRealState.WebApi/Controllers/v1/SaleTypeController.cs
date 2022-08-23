using DRRealState.Core.Application.DTOS.SaleType;
using DRRealState.Core.Application.Features.SaleType.Querys.GetAllSaleTypeQuery;
using DRRealState.Core.Application.Features.SaleType.Querys.GetSaleTypeByIdQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DRRealState.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    public class SaleTypeController : BaseAPIController
    {
        [Authorize(Roles = "ADMINISTRATOR,DEVELOPER")]
        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaleTypeResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {

            try
            {
                return Ok(await Mediator.Send(new GetAllSaleTypeQuery()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [Authorize(Roles = "ADMINISTRATOR,DEVELOPER")]
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaleTypeResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {

            try
            {
                return Ok(await Mediator.Send(new GetSaleTypeByIdQuery() { Id = id }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
