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

namespace DRRealState.Core.Application.Features.Estates.Queries.GetEstatesByCodeQuery
{
    /// <summary>
    /// Parameters to get an Estate By Code
    /// </summary>
    public class GetEstatesByCodeQuery :IRequest<EstatesResponse>
    {
        ///<example>178985</example>
        [SwaggerParameter(Description="Estates Code to Search on the System.")]
        
        public string Code { get; set; }
    }

    public class GetEstateByCodeQueryHandler : IRequestHandler<GetEstatesByCodeQuery, EstatesResponse>
    {
        private readonly IEstateRepository _estatesRepository;
        private readonly IAccountServices _accountServices;
        private readonly IMapper _mapper;

        public GetEstateByCodeQueryHandler(IEstateRepository estatesRepository, IMapper mapper,IAccountServices accountServices)
        {
            _estatesRepository = estatesRepository;
            _mapper = mapper;
            _accountServices = accountServices;
        }

        public async Task<EstatesResponse> Handle(GetEstatesByCodeQuery request, CancellationToken cancellationToken)
        {
            var response = await GetEstatesByCode(request.Code);

            return response;
        }

        private async Task<EstatesResponse> GetEstatesByCode(string Code) {

            var estates = await _estatesRepository.GetAllExtensiveInclude();
            var estatesCode = estates.FirstOrDefault(x => x.Code == Code);
            if (estatesCode == null) { throw new Exception($"Estates not found"); }

            var mapper = _mapper.Map<EstatesResponse>(estatesCode);

            var users = await _accountServices.GetUsersAsync();

            var agent = users.FirstOrDefault(agent => agent.Id == mapper.AgentId);

            mapper.AgentName = $"{agent.FirstName} {agent.LastName}";

            return mapper;
        
        }
    }
}
