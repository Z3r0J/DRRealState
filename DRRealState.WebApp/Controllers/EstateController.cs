using AutoMapper;
using DRRealState.Core.Application.DTOS.Account;
using DRRealState.Core.Application.Helpers;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.Estate;
using DRRealState.Core.Application.ViewModel.Gallery;
using DRRealState.Core.Application.ViewModel.UpgradeEstate;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUpEstateServices _upEstateServices;
        private readonly IPropertiesTypeServices _propertiesTypeServices;
        private readonly ISaleTypeServices _saleTypeServices;
        private readonly IGalleryServices _galleryServices;
        private readonly IMapper _mapper;
        public EstateController(IEstateServices estateServices, 
            IUserServices userServices,
            IUpgradeServices upgradeServices,
            ISaleTypeServices saleType,
            IPropertiesTypeServices propertiesTypeServices,
            IUpEstateServices upEstateServices,
            IGalleryServices galleryServices,
            IMapper mapper)
        {
            _estateServices = estateServices;
            _userServices = userServices;
            _upgradeServices = upgradeServices;
            _propertiesTypeServices = propertiesTypeServices;
            _saleTypeServices = saleType;
            _upEstateServices = upEstateServices;
            _galleryServices = galleryServices;
            _mapper = mapper;
        }
        [Authorize(Roles = "AGENT")]
        public async Task<IActionResult> MyProperty() {

            var agentId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            ViewBag.PropertyType = await _propertiesTypeServices.GetAllViewModel();

            return View(await _estateServices.GetEstateByAgentId(agentId));

        }
        [Authorize(Roles = "AGENT")]
        public async Task<IActionResult> Create() {

            SaveEstateViewModel model = new()
            {
                Upgrades = await _upgradeServices.GetAllViewModel(),
                Properties = await _propertiesTypeServices.GetAllViewModel(),
                SaleTypes = await _saleTypeServices.GetAllViewModel(),

            };

            return View(model);
        }

        [Authorize(Roles = "AGENT")]
        [HttpPost]
        public async Task<IActionResult> Create(SaveEstateViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.Upgrades = await _upgradeServices.GetAllViewModel();
                model.Properties = await _propertiesTypeServices.GetAllViewModel();
                model.SaleTypes = await _saleTypeServices.GetAllViewModel();

                return View(model);

            }

            model.Code = GenerateRandomCode.GenerateCode();

            var Houses = await _estateServices.GetAllViewModel();

            if (Houses.Any(x => x.Code == model.Code))
            {
                ModelState.AddModelError("Description", "Something went wrong try again");

                model.Upgrades = await _upgradeServices.GetAllViewModel();
                model.Properties = await _propertiesTypeServices.GetAllViewModel();
                model.SaleTypes = await _saleTypeServices.GetAllViewModel();

                return View(model);

            }


            if (model.Photos.Count > 4)
            {
                ModelState.AddModelError("Photos", "Please select 1 to 4 photos no more than 4.");
                model.Upgrades = await _upgradeServices.GetAllViewModel();
                model.Properties = await _propertiesTypeServices.GetAllViewModel();
                model.SaleTypes = await _saleTypeServices.GetAllViewModel();

                return View(model);
            }
            else
            {
                model.AgentId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;

                var estate = await _estateServices.Add(model);

                foreach (int UpId in model.UpgradeIds)
                {
                    SaveUpEstateViewModel vm = new() { EstateId = estate.Id, UpgradeId = UpId };

                    await _upEstateServices.Add(vm);
                }

                foreach (var file in model.Photos)
                {
                    SaveGalleryViewModel vm = new()
                    {
                        EstateId = estate.Id,
                        Name = file.FileName,
                        Url = UploadFile(file, estate.Id.ToString())
                    };

                    await _galleryServices.Add(vm);
                }
            }

            return RedirectToAction("MyProperty");
        }

        [Authorize(Roles = "AGENT")]
        public async Task<IActionResult> Edit(int id) {

            var agentId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;

            var estate = await _estateServices.GetViewModelWithIncludeById(id);

            var mapper = _mapper.Map<SaveEstateViewModel>(estate);
            mapper.Upgrades = await _upgradeServices.GetAllViewModel();
            mapper.Properties = await _propertiesTypeServices.GetAllViewModel();
            mapper.SaleTypes = await _saleTypeServices.GetAllViewModel();


            return mapper.AgentId==agentId?View(mapper):RedirectToAction("MyProperty");
        }
        [Authorize(Roles = "AGENT")]
        [HttpPost]
        public async Task<IActionResult> Edit(SaveEstateViewModel model) {

            if (!ModelState.IsValid)
            {
                model.Upgrades = await _upgradeServices.GetAllViewModel();
                model.Properties = await _propertiesTypeServices.GetAllViewModel();
                model.SaleTypes = await _saleTypeServices.GetAllViewModel();

                return View(model);
            }
            if (model.Photos!=null)
            {
                if (model.Photos.Count > 4)
                {
                    ModelState.AddModelError("Photos", "Please select 1 to 4 photos no more than 4.");
                    model.Upgrades = await _upgradeServices.GetAllViewModel();
                    model.Properties = await _propertiesTypeServices.GetAllViewModel();
                    model.SaleTypes = await _saleTypeServices.GetAllViewModel();

                    return View(model);
                }

                else
                {
                    await _estateServices.Update(model, model.Id);

                    if (model.UpgradeIds.Count > 0)
                    {
                        var get = await _upEstateServices.GetAllViewModel();
                        var filter = get.Where(x => x.EstateId == model.Id).ToList();

                        if (filter.Count > 0)
                        {
                            foreach (var item in filter)
                            {
                                await _upEstateServices.Delete(item.Id);
                            }
                        }

                        foreach (int UpId in model.UpgradeIds)
                        {
                            await _upEstateServices.Add(new() { EstateId = model.Id, UpgradeId = UpId });
                        }
                    }
                    foreach (var file in model.Photos)
                    {
                        SaveGalleryViewModel vm = new()
                        {
                            EstateId = model.Id,
                            Name = file.FileName,
                            Url = UploadFile(file, model.Id.ToString())
                        };
                        await _galleryServices.Add(vm);
                    }
                }
            }

            else
                {
                    await _estateServices.Update(model, model.Id);

                    if (model.UpgradeIds.Count > 0)
                    {
                        var get = await _upEstateServices.GetAllViewModel();
                        var filter = get.Where(x => x.EstateId == model.Id).ToList();

                        if (filter.Count > 0)
                        {
                            foreach (var item in filter)
                            {
                                await _upEstateServices.Delete(item.Id);
                            }
                        }

                        foreach (int UpId in model.UpgradeIds)
                        {
                            await _upEstateServices.Add(new() { EstateId = model.Id, UpgradeId = UpId });
                        }
                    }
                }

            return RedirectToRoute(new {action="MyProperty",controller="Estate" });
        }

        [Authorize(Roles = "AGENT")]
        public async Task<IActionResult> Delete(int id) {
            var agentId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;

            var estate = await _estateServices.GetByIdSaveViewModel(id);

            return estate.AgentId == agentId ? View(estate): RedirectToAction("MyProperty");

        }

        [Authorize(Roles = "AGENT")]
        [HttpPost]
        public async Task<IActionResult> Delete(SaveEstateViewModel model) {

            await _estateServices.Delete(model.Id);

            string basePath = $"/Images/Estates/{model.Id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (Directory.Exists(path))
            {
                DirectoryInfo directory = new(path);

                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo folder in directory.GetDirectories())
                {
                    folder.Delete(true);
                }

                Directory.Delete(path);
            }

            return RedirectToRoute(new { action = "MyProperty", controller = "Estate" });

        }


        [Authorize(Roles = "AGENT")]
        [HttpPost]
        public async Task<IActionResult> SearchAdvancedAgent(FilterEstateViewModel filter, string view)
        {

            var agentId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            ViewBag.PropertyType = await _propertiesTypeServices.GetAllViewModel();


            return View(view, _estateServices.FilterAgentHouses(await _estateServices.GetEstateByAgentId(agentId), filter));
        }

        [Authorize(Roles = "AGENT")]
        [HttpPost]
        public async Task<IActionResult> FilterByCodeAgentHouse(string Code, string view)
        {

            if (string.IsNullOrEmpty(Code))
            {
                return RedirectToAction(view);
            }

            var agentId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            var houses = await _estateServices.GetEstateByAgentId(agentId);
            ViewBag.PropertyType = await _propertiesTypeServices.GetAllViewModel();


            return View(view, houses.Where(x => x.Code == Code).ToList());
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
            string basePath = $"/Images/Estates/{id}";
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
