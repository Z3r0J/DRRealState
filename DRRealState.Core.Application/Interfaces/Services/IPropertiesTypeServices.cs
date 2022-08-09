using DRRealState.Core.Application.ViewModel.PropertiesType;
using DRRealState.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Interfaces.Services
{
    public interface IPropertiesTypeServices : IGenericServices
        <SavePropertiesTypeViewModel, PropertiesTypeViewModel, PropertiesType>
    {
        Task<List<PropertiesTypeViewModel>> GetAllViewModelWithInclude();
    }
}
