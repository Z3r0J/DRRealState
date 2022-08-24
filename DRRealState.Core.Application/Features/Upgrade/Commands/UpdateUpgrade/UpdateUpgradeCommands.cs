﻿using AutoMapper;
using DRRealState.Core.Application.DTOS.Upgrade;
using DRRealState.Core.Application.Interfaces.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.Upgrade.Commands.UpdateUpgrade
{
    public class UpdateUpgradeCommands : IRequest<UpgradeResponse>
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class UpdateUpgradeCommandsHandler : IRequestHandler<UpdateUpgradeCommands, UpgradeResponse>
    {
        private readonly IUpgradeRepository _upgradeRepository;
        private readonly IMapper _mapper;
        public UpdateUpgradeCommandsHandler( IUpgradeRepository upgradeRepository, IMapper mapper )
        {
            _upgradeRepository = upgradeRepository;
            _mapper = mapper;
                
        }
        public async Task<UpgradeResponse> Handle(UpdateUpgradeCommands commands, CancellationToken cancellationToken)
        {

            var upgrade = await _upgradeRepository.GetByIdAsync(commands.Id);
            if (upgrade == null) { throw new Exception("Upgrade not Found"); }

            upgrade = _mapper.Map<DRRealState.Core.Domain.Entities.Upgrade>(commands);

            await _upgradeRepository.UpdateAsync(upgrade, upgrade.Id);

            return _mapper.Map<UpgradeResponse>(commands);
            
        }
    }
}
