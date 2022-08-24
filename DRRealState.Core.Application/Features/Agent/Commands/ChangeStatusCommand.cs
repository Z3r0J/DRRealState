using DRRealState.Core.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.Agent.Commands
{
    public class ChangeStatusCommand : IRequest<string>
    {
        public string Id { get; set; }
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
