using GuardingUS.Models;
using GuardingUS.Models.ViewModels;
using GuardingUS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace GuardingUS.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IUserService userService;
        private readonly IUserRepository userRepository;

        public NotificationsController(INotificationRepository notificationRepository, IUserService userService, IUserRepository userRepository)
        {
            this.notificationRepository = notificationRepository;
            this.userService = userService;
            this.userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //var userId = userService.GetUserId();
            var notifications = await notificationRepository.Get(userId);
            return View(notifications);
        }

        public async Task<IActionResult> Message(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userId = userService.GetUserId();
            var message = await notificationRepository.Get(id,userId);
            return View(message);
        }

        [HttpGet]
        public async Task<IActionResult> SendIndividual()
        {
            var model = new AddNotificationVM();
            model.Users = await GetUsers();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddNotificationVM notification)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userId = userService.GetUserId();
            var receiveId = notification.IdUser;
            notification.IdUser = userId;
            await notificationRepository.Create(notification);

            var model = new UserNotifications();
            model.IdUser = receiveId;
            model.IdNotification = notification.Id;

            await notificationRepository.Send(model);
            

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> SendGroup()
        {
            var model = new AddNotificationVM();
            model.Users = await GetUsers();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(AddNotificationVM notification)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userId = userService.GetUserId();
            var receiveId = notification.IdUser;
            notification.IdUser = userId;
            await notificationRepository.Create(notification);

            var model = new UserNotifications();
            model.IdUser = receiveId;
            model.IdNotification = notification.Id;

            await notificationRepository.Send(model);


            return RedirectToAction("Index");
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
