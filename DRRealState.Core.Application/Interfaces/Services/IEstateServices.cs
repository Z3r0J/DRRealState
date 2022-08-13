using DRRealState.Core.Application.ViewModel.Estate;
using DRRealState.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Interfaces.Services
{
    public interface IEstateServices : IGenericServices<SaveEstateViewModel, EstateViewModel, Estate>
    {
        Task<List<EstateViewModel>> GetAllViewModelWithInclude();
        Task<List<EstateViewModel>> FilterAsync(FilterEstateViewModel filter);
        Task<EstateViewModel> GetViewModelWithIncludeById(int id);
    }
}
