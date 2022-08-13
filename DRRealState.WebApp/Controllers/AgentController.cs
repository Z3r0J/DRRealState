using DRRealState.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DRRealState.WebApp.Controllers
{
    public class AgentController : Controller
    {
        private readonly IEstateServices _estateServices;
        public AgentController()
        {

        }

        //public Task<IActionResult> Index()
        //{
        //    return View();
        //}
    }
}
