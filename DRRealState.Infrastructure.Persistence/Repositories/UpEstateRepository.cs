using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Domain.Entities;
using DRRealState.Infrastructure.Persistence.Contexts;


namespace DRRealState.Infrastructure.Persistence.Repositories
{
    public class UpEstateRepository : GenericRepository<Upgrade_Estate>, IUpEstateRepository
    {
        private readonly ApplicationContext _applicationContext;
        public UpEstateRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            _applicationContext = applicationContext;
        }
    }
}
