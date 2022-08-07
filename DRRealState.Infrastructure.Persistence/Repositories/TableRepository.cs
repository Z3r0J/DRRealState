using Microsoft.EntityFrameworkCore;
using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Domain.Entities;
using DRRealState.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Infrastructure.Persistence.Repositories
{
    public class TableRepository : GenericRepository<Table>,ITableRepository
    {
        private readonly ApplicationContext _applicationContext;

        public TableRepository(ApplicationContext applicationContext):base(applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<List<Table>> GetAllExtensiveInclude()
        {
            return await _applicationContext.Set<Table>()
                .Include(x => x.Orders)
                .ThenInclude(di=>di.OrderDishes)
                .ThenInclude(od => od.Dish)
                .ThenInclude(od=>od.DishCategory)
                .Include(x => x.Orders)
                .ThenInclude(di=>di.OrderStatus)
                .Include(x => x.TableStatus).ToListAsync();

        }
    }
}
