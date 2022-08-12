using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.Estate;
using DRRealState.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DRRealState.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEstateServices _estateServices;
        private readonly IAccountServices _accountServices;
        private readonly IPropertiesTypeServices _propertiesTypeServices;
        public HomeController(IEstateServices estateServices, IAccountServices accountServices,IPropertiesTypeServices propertiesType)
        {
            _estateServices = estateServices;
            _accountServices = accountServices;
            _propertiesTypeServices = propertiesType;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _estateServices.GetAllViewModelWithInclude();

            ViewBag.PropertyType = await _propertiesTypeServices.GetAllViewModel();

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> SearchHouseByCode(string Code) { 
        
            var response = await _estateServices.GetAllViewModelWithInclude();

            if (string.IsNullOrEmpty(Code))
            {
                return RedirectToAction("Index");
            }

            ViewBag.PropertyType = await _propertiesTypeServices.GetAllViewModel();


            return View("Index",response.Where(c => c.Code == Code).ToList());
        
        }
        [HttpPost]
        public async Task<IActionResult> AdvancedFilter(FilterEstateViewModel filter) {

            ViewBag.PropertyType = await _propertiesTypeServices.GetAllViewModel();

            return View("Index", await _estateServices.FilterAsync(filter));

        }

        public async Task<IActionResult> Agent() {

            var response = await _accountServices.GetUsersAsync();

            return View(response.Where(x=>x.Roles.Any(r=>r=="AGENT")).ToList());

        }
        [HttpPost]
        public async Task<IActionResult> SearchAgentByName(string Name) {

            var response = await _accountServices.GetUsersAsync();

            return View("Agent",response.Where(x => x.Roles.Any(r => r == "AGENT")&&x.FirstName.Contains(Name)||x.LastName.Contains(Name)).ToList());
        }
    }
}
