using DRRealState.Core.Application.ViewModel.Estate;
using DRRealState.Core.Application.ViewModel.EstateFavorite;
using DRRealState.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Interfaces.Services
{
    public interface IEstateFavoriteServices : IGenericServices<SaveEstateFavoriteViewModel,EstateFavoriteViewModel,EstateFavorite>
    {
        Task<List<EstateFavoriteViewModel>> GetAllViewModelWithInclude();
        List<EstateFavoriteViewModel> AdvancedFilter(List<EstateFavoriteViewModel> favorite, FilterEstateViewModel filter);
    }
}
