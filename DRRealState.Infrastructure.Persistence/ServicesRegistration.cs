using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DRRealState.Infrastructure.Persistence.Repositories;

namespace DRRealState.Infrastructure.Persistence
{
    public static class ServicesRegistration
    {

        public static void AddPersistenceInfrastructure(this IServiceCollection service, IConfiguration configuration) {

            #region Contexts

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                service.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("DRRealStateMemory"));
            }
            else
            {
                service.AddDbContext<ApplicationContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("DRRealStateConnection"), 
                m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            }

            #endregion

            #region Repository

            service.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            #endregion
        }

    }
}
