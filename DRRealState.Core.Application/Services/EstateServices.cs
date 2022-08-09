using AutoMapper;
using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.Estate;
using DRRealState.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Services
{
    public class EstateServices:GenericServices<SaveEstateViewModel,EstateViewModel,Estate>,IEstateServices
    {
        private readonly IEstateRepository _estateRepository;
        public readonly IMapper _mapper;

        public EstateServices(IEstateRepository estateRepository, IMapper mapper):base(estateRepository,mapper)
        {
            _estateRepository = estateRepository;
            _mapper = mapper;
        }

        public async Task<List<EstateViewModel>> GetAllViewModelWithInclude() {

            var estateList = await _estateRepository.GetAllExtensiveInclude();

            return _mapper.Map<List<EstateViewModel>>(estateList);

        }
    }
}
