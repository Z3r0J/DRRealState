using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.SaleType;
using DRRealState.Core.Application.ViewModel.Upgrade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRRealState.WebApp.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR")]
    public class UpgradeController : Controller
    {

        private readonly IUpgradeServices _upgradeServices;

        public UpgradeController(IUpgradeServices upgradeServices)
        {

            _upgradeServices = upgradeServices;
            
        }
        public async Task<IActionResult> Index()
        {
            return View(await _upgradeServices.GetAllViewModelWithInclude());
        }

        //Agregar
        public async Task<IActionResult> Form()
        {
            return View("Form", new SaveUpgradeViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Form(SaveUpgradeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
           

            await _upgradeServices.Add(vm);
            return RedirectToRoute(new { controller = "Upgrade", action = "Index" });
        }
        
        public async Task<IActionResult> Edit(int id)
        {

            return View("Form", await _upgradeServices.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUpgradeViewModel vm, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            await _upgradeServices.Update(vm,id);
            return RedirectToRoute(new { controller = "Upgrade", action = "Index" });
        }


        public async Task<IActionResult> Delete(int id)
        {

            return View(await _upgradeServices.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
       

            await _upgradeServices.Delete(id);
            return RedirectToRoute(new { controller = "Upgrade", action = "Index" });
        }







    }
}
