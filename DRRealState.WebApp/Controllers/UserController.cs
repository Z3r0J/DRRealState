using DRRealState.Core.Application.DTOS.Account;
using DRRealState.Core.Application.Helpers;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DRRealState.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IEstateFavoriteServices _favoriteServices;
        public UserController(IUserServices userServices,IEstateFavoriteServices favoriteServices)
        {
            _userServices = userServices;
            _favoriteServices = favoriteServices;
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        public IActionResult Register()
        {
            return View(new SaveUserViewModel());
        }

        public async Task<IActionResult> Logout() {

            HttpContext.Session.Remove("user");

            await _userServices.LogOutAsync();

            return RedirectToRoute(new { action="Index", controller="Home"});
        }

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm) {


            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (vm.UserType==1)
            {
                var origin = Request.Headers["origin"];

                var resp = await _userServices.RegisterClient(vm, origin);

                if (resp.HasError)
                {
                    vm.Error = resp.Error;
                    vm.HasError = resp.HasError;
                    return View(vm);
                }

                vm.PhotoURL = UploadFile(vm.Photo, resp.Id);

                await _userServices.AddPhoto(vm.PhotoURL, resp.Id);

            }
            else if (vm.UserType==2)
            {
                var resp = await _userServices.RegisterAgent(vm);

                if (resp.HasError)
                {
                    vm.Error = resp.Error;
                    vm.HasError = resp.HasError;
                    return View(vm);
                }

                vm.PhotoURL = UploadFile(vm.Photo, resp.Id);

                await _userServices.AddPhoto(vm.PhotoURL, resp.Id);
            }

            return RedirectToRoute(new { action="Login",controller="User"});

        }

        public IActionResult AccessDenied() {

            return View();
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
                return RedirectToRoute(new { action = "Index", controller = "Administrator" });
            }
            
            else if (response.Roles.Any(x=>x=="AGENT"))
            {
                return RedirectToRoute(new { action = "Index", controller = "Agent" });
            }

            HttpContext.Session.Set<AuthenticationResponse>("user", response);


            return RedirectToRoute(new { action="Index",controller="Home"});
        }

        [Authorize(Roles="CLIENT")]
        public async Task<IActionResult> MyFavorite() {

            var clientId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;

            var house = await _favoriteServices.GetAllViewModelWithInclude();

            return View(house.Where(x=>x.ClientId == clientId).ToList());
        
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string response = await _userServices.ConfirmEmailAsync(userId, token);
            return View("ConfirmEmail", response);
        }

        private string UploadFile(IFormFile file, string id, bool isEditMode = false, string ImagePath = "")
        {


            if (isEditMode)
            {
                if (file == null)
                {
                    return ImagePath;
                }
            }
            string basePath = $"/Images/User/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //create folder if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //get file extension
            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode)
            {
                string[] oldImagePart = ImagePath.Split("/");
                string oldImagePath = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImagePath);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }
            return $"{basePath}/{fileName}";

        }

    }
}
