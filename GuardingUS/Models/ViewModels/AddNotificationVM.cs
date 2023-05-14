using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuardingUS.Models.ViewModels
{
    public class AddNotificationVM : Notifications
    {
        //public int IdUser { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }

       // public List<Users> GroupUsers { get; set; }
    }
}
