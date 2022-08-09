using DRRealState.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Interfaces.Repository
{
    public interface IEstateRepository : IGenericRepository<Estate>
    {
        Task<List<Estate>> GetAllExtensiveInclude();
    }
}
