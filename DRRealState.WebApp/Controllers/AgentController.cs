using DRRealState.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
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

        //public Task<IActionResult> Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Details(string id) {

            var agentList = await _userServices.GetAllUserAsync();

            var estateList = await _estateServices.GetAllViewModelWithInclude();

            ViewBag.Estate = estateList.FindAll(x => x.AgentId == id);

            return View(agentList.FirstOrDefault(a => a.Id == id));
        }
    }
}
