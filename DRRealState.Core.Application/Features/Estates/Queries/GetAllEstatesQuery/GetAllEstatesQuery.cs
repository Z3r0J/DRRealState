using AutoMapper;
using DRRealState.Core.Application.DTOS.Estates;
using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.Estates.Queries.GetAllEstatesQuery
{
    public class GetAllEstatesQuery : IRequest<IList<EstatesResponse>>
    {
    }

    public class GetAllEstatesQueryHandler : IRequestHandler<GetAllEstatesQuery, IList<EstatesResponse>> {

        private readonly IEstateRepository _estateRepository;
        private readonly IAccountServices _accountServices;
        private readonly IMapper _mapper;
        public GetAllEstatesQueryHandler(IEstateRepository estateRepository, IMapper mapper, IAccountServices accountServices)
        {
            _estateRepository = estateRepository;
            _accountServices = accountServices;
            _mapper = mapper;
        }

        public async Task<IList<EstatesResponse>> Handle(GetAllEstatesQuery request, CancellationToken cancellationToken)
        {
            var response = await GetAllResponseWithInclude();

            return response;
        }

        private async Task<List<EstatesResponse>> GetAllResponseWithInclude() {

            var estates = await _estateRepository.GetAllExtensiveInclude();

            if (estates.Count == 0 || estates == null) { throw new Exception($"Estates not found");  }

            var mapper = _mapper.Map<IList<EstatesResponse>>(estates);

            foreach (var item in mapper)
            {
                var users = await _accountServices.GetUsersAsync();

                item.AgentName = users
                    .Select(x => new { Name = $"{x.FirstName} {x.LastName}", x.Id })
                    .FirstOrDefault(x => x.Id == item.AgentId).Name;
            }

            return mapper.ToList();
        
        }
    }
}
