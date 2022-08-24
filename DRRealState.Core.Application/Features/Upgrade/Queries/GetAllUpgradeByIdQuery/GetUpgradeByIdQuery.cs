using AutoMapper;
using DRRealState.Core.Application.DTOS.Upgrade;
using DRRealState.Core.Application.Interfaces.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.Upgrade.Queries.GetAllUpgradeByIdQuery
{
    public class GetUpgradeByIdQuery : IRequest<UpgradeResponse>
    {
        public int Id { get; set; }
    }
    public class GetUgradeByIdQueryHandler : IRequestHandler<GetUpgradeByIdQuery, UpgradeResponse>
    {
        private readonly IUpgradeRepository _upgradeRepository;
        private readonly IMapper _mapper;
        public GetUgradeByIdQueryHandler( IUpgradeRepository upgradeRepository, IMapper mapper)
        {
            _upgradeRepository = upgradeRepository;
            _mapper = mapper;
        }
        public async Task<UpgradeResponse> Handle(GetUpgradeByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await GetUpgradeById(request.Id);
            return response;

        }
        private async Task<UpgradeResponse> GetUpgradeById(int id)
        {
            var UpgradeList = await _upgradeRepository.GetAllAsync();
            var Upgrade = UpgradeList.FirstOrDefault(u => u.Id == id);

            if (Upgrade == null)
            {
                throw new Exception("Upgrade Not Found");
            }

            return _mapper.Map<UpgradeResponse>(Upgrade);
            

            

        }
    }

}
