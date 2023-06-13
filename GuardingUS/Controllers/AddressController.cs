using GuardingUS.Models;
using GuardingUS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GuardingUS.Controllers
{
    [Authorize(Roles = Constants.RoleAdmin)]
    public class AddressController : Controller
    {
        private readonly IAddressRepository addressRepository;

        public AddressController(IAddressRepository addressRepository)
        {
            this.addressRepository = addressRepository;
        }
        public async Task<IActionResult> Index()
        {
            var address = await addressRepository.Get();
            return View(address);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Address address)
        {
            if (!ModelState.IsValid)
            {
                return View(address);
            }

            await addressRepository.Create(address);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var address = await addressRepository.GetById(id);
            return View(address);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Address editAddress)
        {
            await addressRepository.Update(editAddress);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var address = await addressRepository.GetById(id);
            return View(address);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAddress(string id)
        {
            await addressRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
