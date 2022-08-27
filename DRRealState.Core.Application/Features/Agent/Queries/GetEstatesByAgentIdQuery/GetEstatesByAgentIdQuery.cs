using AutoMapper;
using DRRealState.Core.Application.DTOS.Estates;
using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Application.Interfaces.Services;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.Agent.Queries.GetEstatesByAgentIdQuery
{
    /// <summary>
    /// Parameters to Get all related Estates By Agent Id.
    /// </summary>
    public class GetEstatesByAgentIdQuery : IRequest<IList<EstatesResponse>>
    {
        ///<example>Id= 2</example>
        [SwaggerParameter(Description = "Agent Id to search all related estates on the system.")]
        public string AgentId { get; set; }
    }

    public class GetEstatesByAgentIdQueryHandler : IRequestHandler<GetEstatesByAgentIdQuery, IList<EstatesResponse>>
    {
        private readonly IEstateRepository _estateRepository;
        private readonly IAccountServices _accountServices;
        private readonly IMapper _mapper;
        public GetEstatesByAgentIdQueryHandler(IEstateRepository estateRepository, IAccountServices accountServices, IMapper mapper)
        {
            _estateRepository = estateRepository;
            _accountServices = accountServices;
            _mapper = mapper;
        }

        public async Task<IList<EstatesResponse>> Handle(GetEstatesByAgentIdQuery request, CancellationToken cancellationToken)
        {
            var response = await GetEstatesByAgentId(request.AgentId);

            return response;
        }

        private async Task<List<EstatesResponse>> GetEstatesByAgentId(string Id) {

            var estates =await _estateRepository.GetAllExtensiveInclude();

            var estatesByAgent = estates.FindAll(x => x.AgentId == Id);

            if (estatesByAgent == null || estatesByAgent.Count == 0) { throw new Exception("Estates not found."); }

            var mapper = _mapper.Map<List<EstatesResponse>>(estatesByAgent);
            var users = await _accountServices.GetUsersAsync();

            foreach (var item in mapper)
            {
                item.AgentName = users.Select(x => new { Name = $"{x.FirstName} {x.LastName}", x.Id }).FirstOrDefault(a => a.Id == item.AgentId).Name;
            }

            return mapper;
        
        }

    }
}
