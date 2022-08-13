using DRRealState.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DRRealState.WebApp.Controllers
{
    public class EstateController : Controller
    {
        private readonly IEstateServices _estateServices;
        private readonly IUserServices _userServices;
        public EstateController(IEstateServices estateServices, IUserServices userServices)
        {
            _estateServices = estateServices;
            _userServices = userServices;
        }

        public async Task<IActionResult> Details(int Id)
        {
            var response = await _estateServices.GetViewModelWithIncludeById(Id);

            var agentList = await _userServices.GetAllUserAsync();

            ViewBag.Agent = agentList.FirstOrDefault(x => x.Id == response.AgentId);

            return View(response);
        }
    }
}
