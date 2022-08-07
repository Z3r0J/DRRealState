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
    public class DefaultDeveloperUser
    {
        public static async Task SeedAsync(UserManager<RealStateUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            RealStateUser devUser = new() {
                UserName = "DefaultDev",
                Name = "Developer",
                LastName = "DR Real State",
                Email = "developer@drrealstate.com.do",
                EmailConfirmed = true,
                PhoneNumber="+1 809 935 0913",
                PhoneNumberConfirmed = true
            };


            if (userManager.Users.All(u=>u.Id!=devUser.Id))
            {
                var user = await userManager.FindByEmailAsync(devUser.Email);

                if (user==null)
                {
                    await userManager.CreateAsync(devUser,"P@ssw0rd");

                    await userManager.AddToRoleAsync(devUser,Roles.DEVELOPER.ToString());
                }
            }
        }
    }
}
