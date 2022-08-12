using AutoMapper;
using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.UpgradeEstate;
using DRRealState.Core.Domain.Entities;

namespace DRRealState.Core.Application.Services
{
    public class UpEstateServices : GenericServices<SaveUpEstateViewModel, UpEstateViewModel, Upgrade_Estate>, IUpEstateServices
    {
        private readonly IUpEstateRepository _upEstateRepository;
        public readonly IMapper _mapper;

        public UpEstateServices(IUpEstateRepository upEstateRepository, IMapper mapper) : base(upEstateRepository, mapper)
        {
            _upEstateRepository = upEstateRepository;
            _mapper = mapper;
        }
    }
}
