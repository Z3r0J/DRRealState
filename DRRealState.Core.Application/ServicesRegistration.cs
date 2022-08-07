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
            service.AddTransient<IIngredientServices, IngredientServices>();
            service.AddTransient<IDishServices, DishServices>();
            service.AddTransient<IDishIngredientServices, DishIngredientServices>();
            service.AddTransient<ITableServices, TableServices>();
            service.AddTransient<IDishOrderServices, DishOrderServices>();
            service.AddTransient<IOrderServices, OrderServices>();
            #endregion

        }
    }
}
