using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Infrastructure.Identity.Contexts;
using DRRealState.Infrastructure.Identity.Entities;
using DRRealState.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Infrastructure.Identity
{
    public static class WebAppServicesRegistration
    {
        public static void AddWebAppIdentityInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {

            #region Contexts

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                service.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase("IdentityMemory"));
            }
            else
            {
                service.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
            }

            #endregion

            #region Identity
            service.AddIdentity<RealStateUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            service.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User/Login";
                options.AccessDeniedPath = "/User/AccessDenied";
            });

            service.AddAuthentication();


            #endregion

            #region Services

            service.AddTransient<IAccountServices, AccountServices>();

            #endregion
        }
    }
}
