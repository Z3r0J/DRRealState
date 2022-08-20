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
    public class AdministratorController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IEstateServices _estateServices;
        private readonly IMapper _mapper;
        public AdministratorController(IEstateServices estateServices,IUserServices userServices,IMapper mapper)
        {
            _estateServices = estateServices;
            _userServices = userServices;
            _mapper = mapper;
        }
        [Authorize(Roles="ADMINISTRATOR")]
        public async Task<IActionResult> Index()
        {

            var estates = await _estateServices.GetAllViewModelWithInclude();
            var users = await _userServices.GetAllUserAsync();

            DashBoardViewModel dashBoard = new();

            dashBoard.Estates = estates.Count;
            dashBoard.AgentActive = users.FindAll(x=>x.Roles.Any(r=>r=="AGENT")&&x.IsVerified==true).Count;
            dashBoard.AgentInactive = users.FindAll(x=>x.Roles.Any(r=>r=="AGENT")&&x.IsVerified==false).Count;
            dashBoard.DeveloperActive = users.FindAll(x=>x.Roles.Any(r=>r=="DEVELOPER")&&x.IsVerified==true).Count;
            dashBoard.DeveloperInactive = users.FindAll(x=>x.Roles.Any(r=>r=="DEVELOPER")&&x.IsVerified==false).Count;
            dashBoard.ClientActive = users.FindAll(x=>x.Roles.Any(r=>r=="CLIENT")&&x.IsVerified==true).Count;
            dashBoard.ClientInactive = users.FindAll(x=>x.Roles.Any(r=>r=="CLIENT")&&x.IsVerified==false).Count;

            return View(dashBoard);
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> List()
        {

            var userList = await _userServices.GetAllUserAsync();

            var adminList = userList.Where(a => a.Roles.Any(r => r == "ADMINISTRATOR")).ToList();

            return View(adminList);

        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public IActionResult Create() {

            return View(new SaveUserViewModel());
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Edit(string Id) {
            var adminId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;

            if (adminId == Id)
            {
                return RedirectToRoute(new { action = "List", controller = "Administrator" });
            }

            var users = await _userServices.GetAllUserAsync();
            var administrator = _mapper.Map<SaveEditViewModel>(users.FirstOrDefault(x => x.Id == Id && x.Roles.Any(x => x == "ADMINISTRATOR")));

            return administrator != null ? View(administrator) : RedirectToRoute(new { action = "List", controller = "Administrator" });
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

            return RedirectToRoute(new { action = "List", controller = "Administrator" });
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        public async Task<IActionResult> Create(SaveUserViewModel save) {

            if (!ModelState.IsValid)
            {
                return View(save);
            }

            var create = await _userServices.RegisterAdministratorAsync(save);

            if (create.HasError)
            {
                save.HasError = create.HasError;
                save.Error = create.Error;
                return View(save);
            }


            return RedirectToRoute(new { action = "List", controller = "Administrator" });
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Deactivate(string Id)
        {
            var adminId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;

            if (adminId == Id)
            {
                return RedirectToRoute(new { action = "List", controller = "Administrator" });
            }


            var users = await _userServices.GetAllUserAsync();
            var administrator = _mapper.Map<SaveUserViewModel>(users.FirstOrDefault(x => x.Id == Id && x.Roles.Any(x => x == "ADMINISTRATOR")));

            return administrator != null ? View(administrator) : RedirectToRoute(new { action = "List", controller = "Administrator" });
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        public async Task<IActionResult> Deactivate(SaveUserViewModel model)
        {

            await _userServices.DeactivateAsync(new() { UserId = model.Id });


            return RedirectToRoute(new { action = "List", controller = "Administrator" });
        }
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Activate(string Id)
        {
            var adminId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;

            if (adminId == Id)
            {
                return RedirectToRoute(new { action = "List", controller = "Administrator" });
            }

            var users = await _userServices.GetAllUserAsync();
            var administrator = _mapper.Map<SaveUserViewModel>(users.FirstOrDefault(x => x.Id == Id && x.Roles.Any(x => x == "ADMINISTRATOR")));

            return administrator != null ? View(administrator) : RedirectToRoute(new { action = "List", controller = "Administrator" });
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        public async Task<IActionResult> Activate(SaveUserViewModel model)
        {

            await _userServices.ActivateAsync(new() { UserId = model.Id });


            return RedirectToRoute(new { action = "List", controller = "Administrator" });
        }
    }
}
