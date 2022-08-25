using AutoMapper;
using DRRealState.Core.Application.DTOS.Agent;
using DRRealState.Core.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.Agent.Queries.GetAllAgentQuery
{
    public class GetAllAgentQuery : IRequest<IList<AgentResponse>>
    {
    }

    public class GetAllAgentQueryHandler : IRequestHandler<GetAllAgentQuery, IList<AgentResponse>>
    {

        private readonly IAccountServices _accountServices;
        private readonly IEstateServices _estateServices;
        private readonly IMapper _mapper;
        public GetAllAgentQueryHandler(IAccountServices accountServices, IEstateServices estateServices,IMapper mapper)
        {
            _accountServices = accountServices;
            _estateServices = estateServices;
            _mapper = mapper;
        }

        public async Task<IList<AgentResponse>> Handle(GetAllAgentQuery request, CancellationToken cancellationToken)
        {
            var response = await GetAllAgentAsync();

            return response;
        }

        private async Task<List<AgentResponse>> GetAllAgentAsync()
        {

            var users = await _accountServices.GetUsersAsync();
            var agents = users.FindAll(x => x.Roles.Any(r => r == "AGENT"));
            if (agents == null || agents.Count == 0) { throw new Exception($"Agents not found."); }

            List<AgentResponse> agentResponses = _mapper.Map<List<AgentResponse>>(agents);

            foreach (var item in agentResponses)
            {
                var houses = await _estateServices.GetEstateByAgentId(item.Id);

                item.HousesQuantity = houses.Count;
            }

            return agentResponses;
        }
    }
}
