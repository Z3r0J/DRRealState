using AutoMapper;
using DRRealState.Core.Application.DTOS.Account;
using DRRealState.Core.Application.Helpers;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.Estate;
using DRRealState.Core.Application.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DRRealState.WebApp.Controllers
{
    public class AgentController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IEstateServices _estateServices;
        private readonly IPropertiesTypeServices _propertiesTypeServices;
        private readonly IMapper _mapper;
        public AgentController(IUserServices userServices,IEstateServices estateServices, IPropertiesTypeServices propertiesTypeServices, IMapper mapper)
        {
            _userServices = userServices;
            _estateServices = estateServices;
            _propertiesTypeServices = propertiesTypeServices;
            _mapper = mapper;
        }
        [Authorize(Roles = "AGENT")]
        public async Task<IActionResult> Index()
        {
            var agentId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            ViewBag.PropertyType = await _propertiesTypeServices.GetAllViewModel();


            return View(await _estateServices.GetEstateByAgentId(agentId));
        }
        [Authorize(Roles = "AGENT")]
        [HttpPost]
        public async Task<IActionResult> SearchAdvancedAgent(FilterEstateViewModel filter,string view) {

            var agentId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            ViewBag.PropertyType = await _propertiesTypeServices.GetAllViewModel();


            return View(view, _estateServices.FilterAgentHouses(await _estateServices.GetEstateByAgentId(agentId), filter));
        }        
        
        [Authorize(Roles = "AGENT")]
        [HttpPost]
        public async Task<IActionResult> FilterByCodeAgentHouse(string Code, string view) {

            if (string.IsNullOrEmpty(Code)) {
                return RedirectToAction(view);
            }

            var agentId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            var houses = await _estateServices.GetEstateByAgentId(agentId);
            ViewBag.PropertyType = await _propertiesTypeServices.GetAllViewModel();


            return View(view, houses.Where(x=>x.Code==Code).ToList());
        }

        [Authorize(Roles = "AGENT")]
        public async Task<IActionResult> Profile() {

            var agentId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            var agentList = await _userServices.GetAllUserAsync();

            var agent = agentList.FirstOrDefault(x=>x.Id==agentId);

            

            return View(_mapper.Map<SaveUserViewModel>(agent));
        
        }

        [HttpPost]
        [Authorize(Roles = "AGENT")]
        public async Task<IActionResult> Profile(SaveUserViewModel model) {

            if (model.Photo!=null)
            {
                if (string.IsNullOrWhiteSpace(model.PhotoURL))
                {
                    model.PhotoURL = UploadFile(model.Photo, model.Id);
                }
                else
                {
                    model.PhotoURL = UploadFile(model.Photo, model.Id, true, model.PhotoURL);
                }
               
            }

            var response = await _userServices.EditAgentAsync(model);
            if (response.HasError)
            {
                
                model.HasError = true;
                model.Error = response.Error;
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string id) {

            var agentList = await _userServices.GetAllUserAsync();

            ViewBag.Estate = await _estateServices.GetEstateByAgentId(id);

            return View(agentList.FirstOrDefault(a => a.Id == id));
        }
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> List() {

            var userList = await _userServices.GetAllUserAsync();

            var agentList = userList.Where(a => a.Roles.Any(r => r == "AGENT")).ToList();

            foreach (UserViewModel viewModel in agentList)
            {
                var houses = await _estateServices.GetEstateByAgentId(viewModel.Id);

                viewModel.HousesQuantity = houses.Count;
            }

            return View(agentList.ToList());

        }
        [Authorize(Roles = "ADMINISTRATOR")]

        public async Task<IActionResult> Delete(string Id) {

            var users = await _userServices.GetAllUserAsync();
            var agent = _mapper.Map<SaveUserViewModel>(users.FirstOrDefault(x => x.Id == Id && x.Roles.Any(x => x == "AGENT")));

            return agent != null ? View(agent) : RedirectToRoute(new { action = "List", controller = "Agent" });
        
        }

        [HttpPost]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Delete(SaveUserViewModel model) {

           var response =  await _userServices.DeleteAsync(model.Id);

            if (response.HasError)
            {
                return View(model);
            }

            var houses = await _estateServices.GetEstateByAgentId(model.Id);

            if (houses.Count > 0)
            {

                foreach (var item in houses)
                {
                    await _estateServices.Delete(item.Id);
                }
            }

            return RedirectToRoute(new { action = "List", controller = "Agent" });
        
        }
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Deactivate(string Id) {

            var users = await _userServices.GetAllUserAsync();
            var agent = _mapper.Map<SaveUserViewModel>(users.FirstOrDefault(x => x.Id == Id && x.Roles.Any(x => x == "AGENT")));

            return agent != null ? View(agent) : RedirectToRoute(new { action="List",controller="Agent"}) ;
        }
        
        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        public async Task<IActionResult> Deactivate(SaveUserViewModel model) {

            await _userServices.DeactivateAsync(new() { UserId = model.Id });
        

            return RedirectToRoute(new { action = "List", controller = "Agent" });
        }
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Activate(string Id) {

            var users = await _userServices.GetAllUserAsync();
            var agent = _mapper.Map<SaveUserViewModel>(users.FirstOrDefault(x => x.Id == Id && x.Roles.Any(x => x == "AGENT")));

            return agent != null ? View(agent) : RedirectToRoute(new { action="List",controller="Agent"}) ;
        }
        
        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        public async Task<IActionResult> Activate(SaveUserViewModel model) {

            await _userServices.ActivateAsync(new() { UserId = model.Id });
        

            return RedirectToRoute(new { action = "List", controller = "Agent" });
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
