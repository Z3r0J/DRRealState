using AutoMapper;
using DRRealState.Core.Application.DTOS.Account;
using DRRealState.Core.Application.Helpers;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.DashBoard;
using DRRealState.Core.Application.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DRRealState.WebApp.Controllers
{
    public class DeveloperController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        public DeveloperController(IUserServices userServices,IMapper mapper)
        {
            _userServices = userServices;
            _mapper = mapper;
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> List()
        {

            var userList = await _userServices.GetAllUserAsync();

            var devList = userList.Where(a => a.Roles.Any(r => r == "DEVELOPER")).ToList();

            return View(devList);

        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public IActionResult Create() {

            return View(new SaveUserViewModel());
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        public async Task<IActionResult> Create(SaveUserViewModel save)
        {

            if (!ModelState.IsValid)
            {
                return View(save);
            }

            var create = await _userServices.RegisterDeveloper(save);

            if (create.HasError)
            {
                save.HasError = create.HasError;
                save.Error = create.Error;
                return View(save);
            }


            return RedirectToRoute(new { action = "List", controller = "Developer" });
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Edit(string Id) {

            var users = await _userServices.GetAllUserAsync();
            var developer = _mapper.Map<SaveEditViewModel>(users.FirstOrDefault(x => x.Id == Id && x.Roles.Any(x => x == "DEVELOPER")));

            return developer != null ? View(developer) : RedirectToRoute(new { action = "List", controller = "Developer" });
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        public async Task<IActionResult> Edit(SaveEditViewModel model) {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.ConfirmPassword))
            {
                if (model.Password==model.ConfirmPassword)
                {
                   var resp = await _userServices.ChangePasswordAsync(new() { NewPassword = model.Password, UserId = model.Id });

                    if (resp.HasError)
                    {
                        model.HasError = resp.HasError;
                        model.Error = resp.Error;
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("ConfirmPassword", "Please the password and Confirm Password doesn't matches.");
                    return View(model);
                }
            }

            var edit = await _userServices.EditAsync(_mapper.Map<SaveUserViewModel>(model));

            if (edit.HasError)
            {
                model.HasError = edit.HasError;
                model.Error = edit.Error;

                return View(model);
            }

            return RedirectToRoute(new { action = "List", controller = "Developer" });
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Deactivate(string Id)
        {
            var users = await _userServices.GetAllUserAsync();
            var developer = _mapper.Map<SaveUserViewModel>(users.FirstOrDefault(x => x.Id == Id && x.Roles.Any(x => x == "DEVELOPER")));

            return developer != null ? View(developer) : RedirectToRoute(new { action = "List", controller = "Developer" });
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        public async Task<IActionResult> Deactivate(SaveUserViewModel model)
        {

            await _userServices.DeactivateAsync(new() { UserId = model.Id });


            return RedirectToRoute(new { action = "List", controller = "Developer" });
        }
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Activate(string Id)
        {

            var users = await _userServices.GetAllUserAsync();
            var developer = _mapper.Map<SaveUserViewModel>(users.FirstOrDefault(x => x.Id == Id && x.Roles.Any(x => x == "DEVELOPER")));

            return developer != null ? View(developer) : RedirectToRoute(new { action = "List", controller = "Developer" });
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        public async Task<IActionResult> Activate(SaveUserViewModel model)
        {

            await _userServices.ActivateAsync(new() { UserId = model.Id });


            return RedirectToRoute(new { action = "List", controller = "Developer" });
        }
    }
}
