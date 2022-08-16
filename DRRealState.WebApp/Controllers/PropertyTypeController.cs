using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.PropertiesType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRRealState.WebApp.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR")]
    public class PropertyType : Controller
    {

        private readonly IPropertiesTypeServices  _propertiesTypeServices;

        public PropertyType(IPropertiesTypeServices  propertiesTypeServices)
        {

            _propertiesTypeServices = propertiesTypeServices;
            
        }
        public async Task<IActionResult> Index()
        {
            return View(await _propertiesTypeServices.GetAllViewModelWithInclude());
        }

        //Agregar
        public async Task<IActionResult> Form()
        {
            return View("Form", new SavePropertiesTypeViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Form(SavePropertiesTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
           

            await _propertiesTypeServices.Add(vm);
            return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
        }
        
        public async Task<IActionResult> Edit(int id)
        {

            return View("Form", await _propertiesTypeServices.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePropertiesTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Form",vm);
            }


            await _propertiesTypeServices.Update(vm,vm.Id);
            return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
        }


        public async Task<IActionResult> Delete(int id)
        {

            return View(await _propertiesTypeServices.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
       

            await _propertiesTypeServices.Delete(id);
            return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
        }







    }
}
