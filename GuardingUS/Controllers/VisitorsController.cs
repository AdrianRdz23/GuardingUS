using GuardingUS.Models;
using GuardingUS.Models.ViewModels;
using GuardingUS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuardingUS.Controllers
{
    
    public class VisitorsController : Controller
    {
        private readonly IVisitorRepository visitorRepository;
        private readonly IHomeRepository homeRepository;
        private readonly IAddressRepository addressRepository;

        public VisitorsController(IVisitorRepository visitorRepository, IHomeRepository homeRepository, IAddressRepository addressRepository)
        {
            this.visitorRepository = visitorRepository;
            this.homeRepository = homeRepository;
            this.addressRepository = addressRepository;
            
        }
        public async Task<IActionResult> Index()
        {
            var vistors = await visitorRepository.Get();
  
            return View(vistors);

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new VisitorCreationVM();
            model.Address = await GetAddress();
            model.Homes = await GetHomes("F04B3A49-2B3E-49EC-AA20-2DFAEBC910D5");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VisitorCreationVM visitors)
        {

            
            await visitorRepository.Create(visitors);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(string id)
        {
            Visitors visitors = new Visitors();
            visitors.Id = id;

            await visitorRepository.Update(visitors);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> GetAddress()
        {
            var accounts = await addressRepository.Search();
            return accounts.Select(x => new SelectListItem(x.Name, x.Id));
        }

        private async Task<IEnumerable<SelectListItem>> GetHomes(string idaddress)
        {
            var homes = await homeRepository.Get(idaddress);
            return homes.Select(x => new SelectListItem(x.Number.ToString(), x.Id));
        }
        [HttpPost]
        public async Task<IActionResult> GetHomes2([FromBody] string idaddress)
        {
            var homes = await GetHomes(idaddress);
            return Ok(homes);
        }
    }
}
