using AutoMapper;
using DRRealState.Core.Application.DTOS.Agent;
using DRRealState.Core.Application.Interfaces.Services;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.Agent.Queries.GetAgentByIdQuery
{
    /// <summary>
    /// Parameters to Get an Agent By Id.
    /// </summary>
    public class GetAgentByIdQuery : IRequest<AgentResponse>
    {
        [SwaggerParameter(Description ="Agent Id to search on the system.")]
        public string Id { get; set; }
    }

    public class GetAgentByIdQueryHandler : IRequestHandler<GetAgentByIdQuery, AgentResponse>
    {
        private readonly IAccountServices _accountServices;
        private readonly IEstateServices _estateServices;
        private readonly IMapper _mapper;
        public GetAgentByIdQueryHandler(IAccountServices accountServices,IEstateServices estateServices,IMapper mapper)
        {
            _accountServices = accountServices;
            _estateServices = estateServices;
            _mapper = mapper;
        }

        public async Task<AgentResponse> Handle(GetAgentByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await GetAgentById(request.Id);

            return response;
        }

        private async Task<AgentResponse> GetAgentById(string Id) {

            var users = await _accountServices.GetUsersAsync();

            var agent = users.FirstOrDefault(x => x.Roles.Any(r => r == "AGENT") && x.Id == Id);

            if (agent == null) { throw new Exception($"Agent not found."); }

            var houses = await _estateServices.GetEstateByAgentId(agent.Id);
            var response = _mapper.Map<AgentResponse>(agent);
            response.HousesQuantity = houses.Count;

            return response;
        }
    }
}
