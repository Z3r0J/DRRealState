using DRRealState.Core.Application.DTOS.Account;
using DRRealState.Core.Application.Helpers;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.Estate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DRRealState.WebApp.Controllers
{
    public class EstateController : Controller
    {
        private readonly IEstateServices _estateServices;
        private readonly IUserServices _userServices;
        private readonly IUpgradeServices _upgradeServices;
        private readonly IPropertiesTypeServices _propertiesTypeServices;
        private readonly ISaleTypeServices _saleTypeServices;
        public EstateController(IEstateServices estateServices, IUserServices userServices,IUpgradeServices upgradeServices,ISaleTypeServices saleType,IPropertiesTypeServices propertiesTypeServices)
        {
            _estateServices = estateServices;
            _userServices = userServices;
            _upgradeServices = upgradeServices;
            _propertiesTypeServices = propertiesTypeServices;
            _saleTypeServices = saleType;
        }

        public async Task<IActionResult> MyProperty() {

            var agentId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;

            return View(await _estateServices.GetEstateByAgentId(agentId));

        }

        public async Task<IActionResult> Create() {

            SaveEstateViewModel model = new()
            {
                Upgrades = await _upgradeServices.GetAllViewModel(),
                Properties = await _propertiesTypeServices.GetAllViewModel(),
                SaleTypes = await _saleTypeServices.GetAllViewModel(),

            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(SaveEstateViewModel model) {

            if (!ModelState.IsValid)
            {
                model.Upgrades = await _upgradeServices.GetAllViewModel();
                model.Properties = await _propertiesTypeServices.GetAllViewModel();
                model.SaleTypes = await _saleTypeServices.GetAllViewModel();

                return View(model);

            }

            return View();
        
        }

        public async Task<IActionResult> Details(int Id)
        {
            var response = await _estateServices.GetViewModelWithIncludeById(Id);

            var agentList = await _userServices.GetAllUserAsync();

            ViewBag.Agent = agentList.FirstOrDefault(x => x.Id == response.AgentId);

            return View(response);
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
