﻿using DRRealState.Core.Application.DTOS.Account;
using DRRealState.Core.Application.Helpers;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.Estate;
using DRRealState.Core.Application.ViewModel.EstateFavorite;
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
        private readonly IUserServices _accountServices;
        private readonly IPropertiesTypeServices _propertiesTypeServices;
        private readonly IEstateFavoriteServices _favoriteServices;
        public HomeController(IEstateServices estateServices, IUserServices accountServices,IPropertiesTypeServices propertiesType,IEstateFavoriteServices favoriteServices)
        {
            _estateServices = estateServices;
            _accountServices = accountServices;
            _propertiesTypeServices = propertiesType;
            _favoriteServices = favoriteServices;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _estateServices.GetAllViewModelWithInclude();

            ViewBag.Message = "";
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
            ViewBag.Message = "";

            return View("Index",response.Where(c => c.Code == Code).ToList());
        
        }
        [HttpPost]
        public async Task<IActionResult> AdvancedFilter(FilterEstateViewModel filter) {

            ViewBag.PropertyType = await _propertiesTypeServices.GetAllViewModel();
            ViewBag.Message = "";

            return View("Index", await _estateServices.FilterAsync(filter));

        }

        public async Task<IActionResult> Agent() {

            var response = await _accountServices.GetAllUserAsync();

            return View(response.Where(x=>x.Roles.Any(r=>r=="AGENT")&&x.IsVerified==true).OrderBy(x=>x.FirstName).ToList());

        }
        [HttpPost]
        public async Task<IActionResult> SearchAgentByName(string Name) {

            var response = await _accountServices.SearchAgentAsync(Name);

                return View("Agent", response);
        }

        [Authorize(Roles = "CLIENT")]
        public async Task<IActionResult> Favorite(int houseId) {

            var fav = await _favoriteServices.GetAllViewModel();
            var clientId = HttpContext.Session.Get<AuthenticationResponse>("user").Id;

            var exist = fav.Find(x => x.EstateId == houseId && x.ClientId == x.ClientId);

            var response = await _estateServices.GetAllViewModelWithInclude();
            ViewBag.PropertyType = await _propertiesTypeServices.GetAllViewModel();

            if (exist!=null)
            {

                await _favoriteServices.Delete(exist.Id);

                ViewBag.Message = "The favorite was delete successfully!";

                return View("Index",response);
            }

            SaveEstateFavoriteViewModel model = new() { ClientId = clientId, EstateId = houseId };

            await _favoriteServices.Add(model);

            ViewBag.Message = "The favorite was added successfully!";

            return View("Index",response);
        }
    }
}
