using DRRealState.Core.Application.Interfaces.Services;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.Agent.Commands
{
    /// <summary>
    /// Parameters to Change Agent Status
    /// </summary>
    public class ChangeStatusCommand : IRequest<string>
    {
        ///<example>Id= 232</example>
        [SwaggerParameter(Description ="Agent Id to Change Status")]
        public string Id { get; set; }

        ///<example>True/False</example>
        [SwaggerParameter(Description = "True or False")]
        public bool Status { get; set; }
    }

    public class ChangeStatusCommandHandler : IRequestHandler<ChangeStatusCommand, string>
    {
        private readonly IAccountServices _accountServices;
        public ChangeStatusCommandHandler(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        public async Task<string> Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
        {
            var response = await ChangeStatusAsync(request);

            return response;
        }

        public async Task<string> ChangeStatusAsync(ChangeStatusCommand command) {

            if (command.Status)
            {
                var response = await _accountServices.ActivateAsync(new() { UserId = command.Id});

                if (response.HasError)
                {
                    throw new Exception(response.Error);
                }
            }
            else
            {
                var response = await _accountServices.DeactivateAsync(new() { UserId = command.Id });

                if (response.HasError)
                {
                    throw new Exception(response.Error);
                }
            }

            return command.Id;
        }
    }
}
