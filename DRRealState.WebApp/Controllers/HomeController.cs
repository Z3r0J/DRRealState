using DRRealState.Core.Application.Interfaces.Services;
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

        public HomeController(IEstateServices estateServices)
        {
            _estateServices = estateServices;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _estateServices.GetAllViewModelWithInclude();

            return View(response);
        }
    }
}
