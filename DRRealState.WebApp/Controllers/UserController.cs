using DRRealState.Core.Application.DTOS.Account;
using DRRealState.Core.Application.Helpers;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRRealState.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public IActionResult Login()
        {


            return View(new LoginViewModel());
        }
        public IActionResult Register()
        {


            return View(new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {

            if (!ModelState.IsValid) { 
            
            return View(vm);
            }    

            var response = await _userServices.LoginWebAppAsync(vm);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;

                return View(vm);
            }


            if (response.Roles.Any(x=>x=="ADMINISTRATOR"))
            {
                return RedirectToRoute(new { action = "Administrator", controller = "Home" });
            }
            
            else if (response.Roles.Any(x=>x=="AGENT"))
            {
                return RedirectToRoute(new { action = "Agent", controller = "Home" });
            }

            HttpContext.Session.Set<AuthenticationResponse>("user", response);


            return RedirectToRoute(new { action="Index",controller="Home"});
        }

    }
}
