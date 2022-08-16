using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.SaleType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRRealState.WebApp.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR")]
    public class SaleTypeController : Controller
    {

        private readonly ISaleTypeServices _saleTypeServices;

        public SaleTypeController(ISaleTypeServices saleTypeServices)
        {

            _saleTypeServices = saleTypeServices;
            
        }
        public async Task<IActionResult> Index()
        {
            return View(await _saleTypeServices.GetAllViewModelWithInclude());
        }

        //Agregar
        public IActionResult Form()
        {
            return View("Form", new SaveSaleTypeViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Form(SaveSaleTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
           

            await _saleTypeServices.Add(vm);
            return RedirectToRoute(new { controller = "SaleType", action = "Index" });
        }
        
        public async Task<IActionResult> Edit(int id)
        {

            return View("Form", await _saleTypeServices.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveSaleTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            await _saleTypeServices.Update(vm,vm.Id);
            return RedirectToRoute(new { controller = "SaleType", action = "Index" });
        }


        public async Task<IActionResult> Delete(int id)
        {

            return View(await _saleTypeServices.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
       

            await _saleTypeServices.Delete(id);
            return RedirectToRoute(new { controller = "SaleType", action = "Index" });
        }







    }
}
