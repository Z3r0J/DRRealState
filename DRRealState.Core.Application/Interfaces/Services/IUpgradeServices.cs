using DRRealState.Core.Application.ViewModel.Upgrade;
using DRRealState.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Interfaces.Services
{
    public interface IUpgradeServices : IGenericServices<SaveUpgradeViewModel, UpgradeViewModel, Upgrade>
    {
        Task<List<UpgradeViewModel>> GetAllViewModelWithInclude();

    }
}
