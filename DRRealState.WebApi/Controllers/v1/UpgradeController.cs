using DRRealState.Core.Application.DTOS.Upgrade;
using DRRealState.Core.Application.Features.Upgrade.Commands.CreateUpgrade;
using DRRealState.Core.Application.Features.Upgrade.Commands.DeleteUpgrade;
using DRRealState.Core.Application.Features.Upgrade.Commands.UpdateUpgrade;
using DRRealState.Core.Application.Features.Upgrade.Queries;
using DRRealState.Core.Application.Features.Upgrade.Queries.GetAllUpgradeByIdQuery;
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
    [SwaggerTag("Upgrade Maintenance")]
    public class UpgradeController : BaseAPIController
    {
        [Authorize(Roles = "ADMINISTRATOR,DEVELOPER")]
        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpgradeResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "List of Upgrades",
            Description = "Get a List of Upgrades register on the system. (Only Administrator and Developer can use this endpoint)")]
        public async Task<IActionResult> Get()
        {

            try
            {
                return Ok(await Mediator.Send(new GetAllUpgradeQuery()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [Authorize(Roles = "ADMINISTRATOR,DEVELOPER")]
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpgradeResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Upgrade By Id",
            Description = "Get an Upgrade with this Id register on the system. (Only Administrator and Developer can use this endpoint)")]
        public async Task<IActionResult> Get(int id)
        {

            try
            {
                return Ok(await Mediator.Send(new GetUpgradeByIdQuery() {Id=id }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost("Create/Upgrade")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Create a Upgrade",
            Description = "Create a new Upgrade on the system. (Only Administrator can use this endpoint)")]
        public async Task<IActionResult> Post([FromBody] CreateUpgradeCommands command)
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpgradeResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Update an Upgrade",
            Description = "Update an existing Upgrade with new Value on the system. (Only Administrator can use this endpoint)")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateUpgradeCommands command)
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
            Summary = "Delete a Upgrade",
            Description = "Delete an Upgrade on the system. (Only Administrator can use this endpoint)")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await Mediator.Send(new DeleteUpgradeCommand { Id = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
