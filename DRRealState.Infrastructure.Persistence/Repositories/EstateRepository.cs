using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Domain.Entities;
using DRRealState.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Infrastructure.Persistence.Repositories
{
    public class EstateRepository : GenericRepository<Estate>,IEstateRepository
    {
        private readonly ApplicationContext _applicationContext;

        public EstateRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<List<Estate>> GetAllExtensiveInclude() {

            return await _applicationContext.Set<Estate>()
                   .Include(x=>x.Gallery)
                   .Include(x=>x.Upgrade)
                   .ThenInclude(x=>x.Upgrade)
                   .Include(x=>x.SaleType)
                   .Include(x=>x.PropertiesType)
                   .Include(x=>x.Favorites)
                   .ToListAsync();
        }
    }
}
