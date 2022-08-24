using AutoMapper;
using DRRealState.Core.Application.Interfaces.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DRRealState.Core.Domain.Entities;

namespace DRRealState.Core.Application.Features.Upgrade.Commands.CreateUpgrade
{
    public class CreateUpgradeCommands : IRequest<int>
    {
     
        public string Name { get; set; }
        public string Description { get; set; }



    }
    public class CreateUpgradeCommandsHandler : IRequestHandler<CreateUpgradeCommands, int>
    {
        private readonly IUpgradeRepository _upgradeRepository;
        private readonly IMapper _mapper;
        public CreateUpgradeCommandsHandler(IUpgradeRepository upgradeRepository, IMapper mapper)
        {
            _upgradeRepository = upgradeRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpgradeCommands command, CancellationToken cancellationToken)
        {
           var upgrade = _mapper.Map< DRRealState.Core.Domain.Entities.Upgrade> (command);

           var create =  await _upgradeRepository.AddAsync(upgrade);


            return create.Id;
        }
    }
}
