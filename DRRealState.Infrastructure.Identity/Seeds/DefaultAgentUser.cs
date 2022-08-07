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
    public class DefaultAgentUser
    {
        public static async Task SeedAsync(UserManager<RealStateUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            RealStateUser agentUser = new() {
                UserName = "DefaultAgent",
                Name = "Client",
                LastName = "DR REAL STATE",
                Email = "client@drrealstate.com.do",
                EmailConfirmed = true,
                PhoneNumber="+1 809 935 0913",
                PhoneNumberConfirmed = true
            };


            if (userManager.Users.All(u=>u.Id!=agentUser.Id))
            {
                var user = await userManager.FindByEmailAsync(agentUser.Email);

                if (user==null)
                {
                    await userManager.CreateAsync(agentUser,"P@ssw0rd");

                    await userManager.AddToRoleAsync(agentUser,Roles.AGENT.ToString());
                }
            }
        }
    }
}
