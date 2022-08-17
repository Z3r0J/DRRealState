using DRRealState.Core.Application.ViewModel.Gallery;
using DRRealState.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Interfaces.Services
{
    public interface IGalleryServices : IGenericServices<SaveGalleryViewModel, GalleryViewModel, Gallery>
    {
        Task<List<GalleryViewModel>> GetAllViewModelWithInclude();

    }
}
