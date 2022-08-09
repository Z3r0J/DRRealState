using DRRealState.Core.Application.ViewModel.SaleType;
using DRRealState.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Interfaces.Services
{
    public interface ISaleTypeServices : IGenericServices<SaveSaleTypeViewModel, SaleTypeViewModel, SaleType>
    {
        Task<List<SaleTypeViewModel>> GetAllViewModelWithInclude();
    }
}
