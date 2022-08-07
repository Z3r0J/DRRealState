using DRRealState.Core.Application.Enums;
using DRRealState.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<RealStateUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new(Roles.DEVELOPER.ToString()));
            await roleManager.CreateAsync(new(Roles.ADMINISTRATOR.ToString()));            
            await roleManager.CreateAsync(new(Roles.AGENT.ToString()));            
            await roleManager.CreateAsync(new(Roles.CLIENT.ToString()));


        }
    }
}
