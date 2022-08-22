using DRRealState.Core.Application.DTOS.Agent;
using DRRealState.Core.Application.DTOS.Estates;
using DRRealState.Core.Application.Features.Agent.Queries.GetAgentByIdQuery;
using DRRealState.Core.Application.Features.Agent.Queries.GetAllAgentQuery;
using DRRealState.Core.Application.Features.Agent.Queries.GetEstatesByAgentIdQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DRRealState.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    public class AgentController : BaseAPIController
    {
        [Authorize(Roles = "ADMINISTRATOR,DEVELOPER")]
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
    }
}
