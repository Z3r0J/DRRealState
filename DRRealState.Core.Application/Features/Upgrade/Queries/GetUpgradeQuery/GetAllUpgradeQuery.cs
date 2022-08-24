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

namespace DRRealState.Core.Application.Features.Upgrade.Queries
{
    public class GetAllUpgradeQuery :IRequest<IList<UpgradeResponse>>
    {

    }
    public class GetAllUpgradeQueryHandler : IRequestHandler<GetAllUpgradeQuery, IList<UpgradeResponse>>
    {
        private readonly IUpgradeRepository _upgradeeRepository;
        private readonly IMapper _mapper;

        public GetAllUpgradeQueryHandler(IUpgradeRepository upgradeRepository, IMapper mapper)
        {
            _upgradeeRepository = upgradeRepository;
            _mapper = mapper;

        }
        public async Task<IList<UpgradeResponse>> Handle(GetAllUpgradeQuery request, CancellationToken cancellationToken)
        {
            var response = await GetAllUpgrade();
            return response;
        }

        private async Task<List<UpgradeResponse>> GetAllUpgrade()
        {

            var UpgradeList = await _upgradeeRepository.GetAllAsync();
            if (UpgradeList.Count == 0 || UpgradeList == null) { throw new Exception("Upgrade no found."); }

            return _mapper.Map<List<UpgradeResponse>>(UpgradeList);

        }
    }
}
