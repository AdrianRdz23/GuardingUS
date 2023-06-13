using AutoMapper;
using GuardingUS.Models;
using GuardingUS.Models.ViewModels;
using GuardingUS.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GuardingUS.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IRoleRepository roleRepository;
        private readonly IUserService userService;
        private readonly ApplicationDbContext context;

        public UsersController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository, IUserService userService, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.roleRepository = roleRepository;
            this.userService = userService;
            this.context = context;
        }

        [Authorize(Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userId = userService.GetUserId();
            var users = await userRepository.Get(userId);
            
            return View(users);
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //var user = new Users() { Name = model.Name,Email = model.Email, PhoneNumber = model.PhoneNumber };
            var user = new IdentityUser() { Email = model.Email, PhoneNumber = model.PhoneNumber, UserName = model.UserName };


            var result = await userManager.CreateAsync(user, password: model.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction("Index", "Visitors");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(model.UserName,
                model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Visitors");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Name of the user or password is incorrect");
                return View(model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Visitors");
        }

        [Authorize(Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await userRepository.GetById(id);
            user.RoleId = await userRepository.GetRoleId(id);

            var model = mapper.Map<EditUserVM>(user);

            model.Roles = await GetRoles();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EditUserVM userEdit)
        {

            //await userRepository.Update(userEdit);
            var usuario = await context.Users.Where(u => u.Id == userEdit.Id).FirstOrDefaultAsync();

            if (userEdit.RoleId == "9A0258A5-D55B-40D8-B67C-F4634B40959E")
            {
                await userManager.AddToRoleAsync(usuario, Constants.RoleAdmin);
            }
            else if (userEdit.RoleId == "A1488B44-508C-4740-B444-87460139EBB7")
            {
                await userManager.AddToRoleAsync(usuario, Constants.RoleGuard);
            }
            else {
                await userManager.AddToRoleAsync(usuario, Constants.RoleResident);
            }
            

            //await userRepository.Update(userEdit);

            return RedirectToAction("Index");

        }
        [Authorize(Roles = Constants.RoleAdmin)]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userRepository.GetById(id);
            user.UserId = user.Id;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            //HW Crear un metodo para editar el owner de la casa y en cambiar el user de las notificaciones
            var userid = id;

            //var user = await context.Users.Where(u => u.Id == userid).FirstOrDefaultAsync();
            //await userManager.SetLockoutEnabledAsync(user, true);
            //await userManager.SetLockoutEndDateAsync(user,DateTime.Today.AddYears(10));

            await userRepository.UpdateUnknown(userid);
            await userRepository.Delete(userid,id);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> GetRoles()
        {
            var accountsTypes = await roleRepository.Get();

            return accountsTypes.Select(x => new SelectListItem(x.Name, x.Id));

        }


    }
}
