using DRRealState.Core.Application.DTOS.Account;
using DRRealState.Core.Application.Helpers;
using DRRealState.Core.Application.Interfaces.Services;
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
        public AgentController(IUserServices userServices,IEstateServices estateServices)
        {
            _userServices = userServices;
            _estateServices = estateServices;
        }
        [Authorize(Roles = "AGENT")]
        public async Task<IActionResult> Index()
        {
            var agentId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;

            return View(await _estateServices.GetEstateByAgentId(agentId));
        }
        [Authorize(Roles = "AGENT")]
        public async Task<IActionResult> Profile() {

            var agentId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            var agentList = await _userServices.GetAllUserAsync();

            var agent = agentList.FirstOrDefault(x=>x.Id==agentId);

            SaveUserViewModel vm = new() {
                Id=agent.Id,
                Name = agent.FirstName,
                LastName = agent.LastName,
                Phone = agent.Phone, 
                PhotoURL = agent.PhotoUrl };

            return View(vm);
        
        }

        [HttpPost]
        [Authorize(Roles = "AGENT")]
        public async Task<IActionResult> Profile(SaveUserViewModel model) {

            if (model.Photo!=null)
            {
                model.PhotoURL = UploadFile(model.Photo, model.Id, true, model.PhotoURL);
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
