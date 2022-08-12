using AutoMapper;
using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.EstateFavorite;
using DRRealState.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Services
{
    public class EstateFavoriteServices : GenericServices<SaveEstateFavoriteViewModel, EstateFavoriteViewModel, EstateFavorite>, IEstateFavoriteServices
    {
        private readonly IEstateFavoriteRepository _estateFavoriteRepository;
        private readonly IMapper _mapper;

        public EstateFavoriteServices(IEstateFavoriteRepository estateFavoriteRepository, IMapper mapper) : base(estateFavoriteRepository, mapper)
        {
            _estateFavoriteRepository = estateFavoriteRepository;
            _mapper = mapper;
        }

        public async Task<List<EstateFavoriteViewModel>> GetAllViewModelWithInclude() {

            var favorite = await _estateFavoriteRepository.GetAllWithIncludeAsync();
            return _mapper.Map<List<EstateFavoriteViewModel>>(favorite);
        
        }
    }
}
