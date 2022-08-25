using DRRealState.Core.Application.DTOS.Agent;
using DRRealState.Core.Application.DTOS.Estates;
using DRRealState.Core.Application.Features.Agent.Commands;
using DRRealState.Core.Application.Features.Agent.Queries.GetAgentByIdQuery;
using DRRealState.Core.Application.Features.Agent.Queries.GetAllAgentQuery;
using DRRealState.Core.Application.Features.Agent.Queries.GetEstatesByAgentIdQuery;
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
    [SwaggerTag("Agent Maintenance")]
    public class AgentController : BaseAPIController
    {
        [Authorize(Roles = "ADMINISTRATOR,DEVELOPER")]
        [SwaggerOperation(Summary ="List of Agent",
            Description ="Get a list of agent register in the system. (Only Administrator and Developer can use this endpoint)")]
        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get() {

            try
            {
                return Ok(await Mediator.Send(new GetAllAgentQuery()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        
        }
        [Authorize(Roles = "ADMINISTRATOR,DEVELOPER")]
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Agent By Id",
            Description = "Get agent with this Id register in the system. (Only Administrator and Developer can use this endpoint)")]
        public async Task<IActionResult> GetByAgentId(string id) {

            try
            {
                return Ok(await Mediator.Send(new GetAgentByIdQuery() { Id=id}));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        
        }
        [Authorize(Roles = "ADMINISTRATOR,DEVELOPER")]
        [HttpGet("GetEstatesByAgentId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EstatesResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "List of Estates By Agent Id",
            Description = "Get estates who the owner is the agent with this Id register in the system. (Only Administrator and Developer can use this endpoint)")]
        public async Task<IActionResult> GetEstatesByAgentId(string id) {

            try
            {
                return Ok(await Mediator.Send(new GetEstatesByAgentIdQuery() { AgentId=id}));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        
        }
        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPatch("ChangeStatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(Summary = "Activate and Deactivate Agent",
            Description = "Using the Id and the new Status you can Activate or Deactivate an Agent in the APP. (Only Administrator can use this endpoint)")]
        public async Task<IActionResult> ChangeStatusAsync([FromBody] ChangeStatusCommand command) {

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
    }
}
