using DRRealState.Core.Application.DTOS.SaleType;
using DRRealState.Core.Application.Features.SaleTypes.Commands.CreateSaleType;
using DRRealState.Core.Application.Features.SaleTypes.Commands.DeleteSaleTypeById;
using DRRealState.Core.Application.Features.SaleTypes.Commands.UpdateSaleType;
using DRRealState.Core.Application.Features.SaleTypes.Queries.GetAllSaleTypeQuery;
using DRRealState.Core.Application.Features.SaleTypes.Queries.GetSaleTypeByIdQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DRRealState.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [SwaggerTag("Sale Type Maintenance")]
    public class SaleTypeController : BaseAPIController
    {
        [Authorize(Roles = "ADMINISTRATOR,DEVELOPER")]
        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaleTypeResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "List of Sale Types",
            Description = "Get a List of Sale Types register on the system. (Only Administrator and Developer can use this endpoint)")]
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
        [SwaggerOperation(
            Summary = "Sale Type By Id",
            Description = "Get a Sale Type with this Id register on the system. (Only Administrator and Developer can use this endpoint)")]
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
        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost("Create/PropertyTypes")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Create a Sale Type",
            Description = "Create a new Sale Type on the system. (Only Administrator can use this endpoint)")]
        public async Task<IActionResult> Post([FromBody] CreateSaleTypeCommand command)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                await Mediator.Send(command);
                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaleTypeUpdateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Update a Sale Type",
            Description = "Update an existing Sale Type with new Value on the system. (Only Administrator can use this endpoint)")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateSaleTypeCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (id != command.Id)
                {
                    return BadRequest();
                }

                return Ok(await Mediator.Send(command));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Delete a Sale Type",
            Description = "Delete a Sale Type in the system and all property related with this sale type. (Only Administrator can use this endpoint)")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await Mediator.Send(new DeleteSaleTypeByIdCommand { Id = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }

}
