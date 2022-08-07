using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Domain.Settings;
using DRRealState.Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DRRealState.Infrastructure.Shared
{
    public static class ServicesRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {

            service.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            service.AddTransient<IEmailServices, EmailServices>();
        }
    }
}
