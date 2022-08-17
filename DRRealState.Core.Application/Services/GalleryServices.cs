using AutoMapper;
using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.Gallery;
using DRRealState.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Services
{
    public class GalleryServices : GenericServices<SaveGalleryViewModel, GalleryViewModel, Gallery>, IGalleryServices
    {
        private readonly IGalleryRepository _upgradeRepository;
        public readonly IMapper _mapper;

        public GalleryServices(IGalleryRepository upgradeRepository, IMapper mapper) : base(upgradeRepository, mapper)
        {
            _upgradeRepository = upgradeRepository;
            _mapper = mapper;
        }
        public async Task<List<GalleryViewModel>> GetAllViewModelWithInclude()
        {
            var listSale = await _upgradeRepository.GetWithIncludeAsync(new() { "Estates" });

            return _mapper.Map<List<GalleryViewModel>>(listSale);
        }
    }
}


