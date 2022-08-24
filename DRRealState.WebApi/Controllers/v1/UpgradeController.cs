﻿using DRRealState.Core.Application.DTOS.PropertiesType;
using DRRealState.Core.Application.DTOS.Upgrade;
using DRRealState.Core.Application.Features.PropertyTypes.Commands.CreatePropertiesType;
using DRRealState.Core.Application.Features.PropertyTypes.Commands.DeletePropertiesTypeById;
using DRRealState.Core.Application.Features.PropertyTypes.Commands.UpdatePropertiesType;
using DRRealState.Core.Application.Features.PropertyTypes.Queries.GetAllPropertiesTypeQuery;
using DRRealState.Core.Application.Features.PropertyTypes.Queries.GetPropertiesTypeByIdQuery;
using DRRealState.Core.Application.Features.Upgrade.Commands.CreateUpgrade;
using DRRealState.Core.Application.Features.Upgrade.Commands.UpdateUpgrade;
using DRRealState.Core.Application.Features.Upgrade.Queries;
using DRRealState.Core.Application.Features.Upgrade.Queries.GetAllUpgradeByIdQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DRRealState.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    public class UpgradeController : BaseAPIController
    {
        [Authorize(Roles = "ADMINISTRATOR,DEVELOPER")]
        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpgradeResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        public async Task<IActionResult> Post(CreateUpgradeCommands command)
        {

            try
            {
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
        public async Task<IActionResult> Put(int id, UpdateUpgradeCommands command)
        {
            try
            {
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


    }
}
