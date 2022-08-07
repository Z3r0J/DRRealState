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
    public class DefaultClientUser
    {
        public static async Task SeedAsync(UserManager<RealStateUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            RealStateUser clientUser = new() {
                UserName = "DefaultClient",
                Name = "Client",
                LastName = "DR REAL STATE",
                Email = "client@drrealstate.com.do",
                EmailConfirmed = true,
                PhoneNumber="+1 809 935 0913",
                PhoneNumberConfirmed = true
            };


            if (userManager.Users.All(u=>u.Id!=clientUser.Id))
            {
                var user = await userManager.FindByEmailAsync(clientUser.Email);

                if (user==null)
                {
                    await userManager.CreateAsync(clientUser,"P@ssw0rd");

                    await userManager.AddToRoleAsync(clientUser,Roles.CLIENT.ToString());
                }
            }
        }
    }
}
