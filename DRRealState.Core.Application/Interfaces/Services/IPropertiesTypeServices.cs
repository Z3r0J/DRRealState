using DRRealState.Core.Application.ViewModel.PropertiesType;
using DRRealState.Core.Domain.Entities;

namespace DRRealState.Core.Application.Interfaces.Services
{
    public interface IPropertiesTypeServices : IGenericServices
        <SavePropertiesTypeViewModel, PropertiesTypeViewModel, PropertiesType>
    {

    }
}
