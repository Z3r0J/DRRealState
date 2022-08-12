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
    public class EstateFavoriteRepository : GenericRepository<EstateFavorite>, IEstateFavoriteRepository
    {
        private readonly ApplicationContext _applicationContext;
        public EstateFavoriteRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<List<EstateFavorite>> GetAllWithIncludeAsync() {

           return await _applicationContext
                .Set<EstateFavorite>().Include(x => x.Estate)
                .ThenInclude(x=>x.PropertiesType)
                .Include(x=>x.Estate)
                .ThenInclude(x=>x.SaleType)
                .Include(x=>x.Estate)
                .ThenInclude(x=>x.Gallery)
                .ToListAsync();
        }
    }
}
