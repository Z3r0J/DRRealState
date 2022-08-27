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

namespace DRRealState.Core.Application.Features.Estates.Queries.GetEstatesByIdQuery
{
    /// <summary>
    /// Parameters to Get an Estates By Id
    /// </summary>
    public class GetEstatesByIdQuery : IRequest<EstatesResponse>
    {
        ///<example> Id= 1</example>
        [SwaggerParameter(Description = "Estates Id to Search on the System.")]
        public int Id { get; set; }
    }

    public class GetEstatesByIdQueryHandler : IRequestHandler<GetEstatesByIdQuery, EstatesResponse>
    {
        private readonly IEstateRepository _estatesRepository;
        private readonly IAccountServices _accountServices;
        private readonly IMapper _mapper;

        public GetEstatesByIdQueryHandler(IEstateRepository estatesRepository, IMapper mapper, IAccountServices accountServices)
        {
            _estatesRepository = estatesRepository;
            _mapper = mapper;
            _accountServices = accountServices;
        }

        public async Task<EstatesResponse> Handle(GetEstatesByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await GetEstatesById(request.Id);

            return response;
        }

        private async Task<EstatesResponse> GetEstatesById(int Id)
        {

            var estates = await _estatesRepository.GetAllExtensiveInclude();
            var estatesId = estates.FirstOrDefault(x => x.Id == Id);
            if (estatesId == null) { throw new Exception($"Estates not found"); }

            var mapper = _mapper.Map<EstatesResponse>(estatesId);

            var users = await _accountServices.GetUsersAsync();

            var agent = users.FirstOrDefault(agent => agent.Id == mapper.AgentId);

            mapper.AgentName = $"{agent.FirstName} {agent.LastName}";

            return mapper;

        }
    }
}
