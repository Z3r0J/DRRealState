using AutoMapper;
using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.Upgrade;
using DRRealState.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Services
{
    public class UpgradeServices : GenericServices<SaveUpgradeViewModel, UpgradeViewModel, Upgrade>, IUpgradeServices
    {
        private readonly IUpgradeRepository _upgradeRepository;
        public readonly IMapper _mapper;

        public UpgradeServices(IUpgradeRepository upgradeRepository, IMapper mapper) : base(upgradeRepository, mapper)
        {
            _upgradeRepository = upgradeRepository;
            _mapper = mapper;
        }
        public async Task<List<UpgradeViewModel>> GetAllViewModelWithInclude()
        {
            var listSale = await _upgradeRepository.GetWithIncludeAsync(new() { "Estates" });

            return _mapper.Map<List<UpgradeViewModel>>(listSale);
        }
    }
}


