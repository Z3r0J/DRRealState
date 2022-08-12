using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DRRealState.Core.Application
{
    public static class ServicesRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection service) 
        {
            service.AddAutoMapper(Assembly.GetExecutingAssembly());

            #region Services
            service.AddTransient(typeof(IGenericServices<,,>), typeof(GenericServices<,,>));
            service.AddTransient<IPropertiesTypeServices, PropertiesTypeServices>();
            service.AddTransient<ISaleTypeServices, SaleTypeServices>();
            service.AddTransient<IUserServices, UserServices>();
            service.AddTransient<IEstateServices, EstateServices>();
            service.AddTransient<IUpgradeServices, UpgradeServices>();
            service.AddTransient<IUpEstateServices, UpEstateServices>();
            service.AddTransient<IEstateFavoriteServices, EstateFavoriteServices>();
            #endregion

        }
    }
}
