using AutoMapper;
using GuardingUS.Models.ViewModels;
using GuardingUS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace GuardingUS.Controllers
{
    public class HomesController : Controller
    {
        private readonly IHomeRepository homeRepository;
        private readonly IAddressRepository addressRepository;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IUserService userService;

        public HomesController(IHomeRepository homeRepository, IAddressRepository addressRepository, IMapper mapper, IUserRepository userRepository, IUserService userService)
        {
            this.homeRepository = homeRepository;
            this.addressRepository = addressRepository;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var homes = await homeRepository.Get();
            return View(homes);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new AddHomeVM();
            model.Address = await GetAddress();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddHomeVM home)
        {
            await homeRepository.Create(home);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var home = await homeRepository.GetById(id);

            //var model = mapper.Map<AddHomeVM>(home);

            var model = new AddHomeVM()
            {
                Id = home.Id,
                Number = home.Number,
                Cars = home.Cars,
                IdAddress = home.IdAddress,
                IdUser = home.IdUser

            };

            model.Address = await GetAddress();
            model.Owners = await GetUsers();

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Update(AddHomeVM homeEdit)
        {
            await homeRepository.Update(homeEdit);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var home = await homeRepository.GetById(id);
            return View(home);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHome(string id)
        {
            await homeRepository.Delete(id);
            return RedirectToAction("Index");
        }



        private async Task<IEnumerable<SelectListItem>> GetAddress()
        {
            var accountsTypes = await addressRepository.Get();

            return accountsTypes.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

        }

        private async Task<IEnumerable<SelectListItem>> GetUsers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //var userId = userService.GetUserId();
            var users = await userRepository.Get(userId);

            return users.Select(x => new SelectListItem(x.UserName, x.Id.ToString()));

        }




    }
}
