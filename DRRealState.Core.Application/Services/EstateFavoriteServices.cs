using AutoMapper;
using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.EstateFavorite;
using DRRealState.Core.Domain.Entities;

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
    }
}
