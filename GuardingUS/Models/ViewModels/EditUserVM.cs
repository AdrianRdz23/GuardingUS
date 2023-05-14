using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuardingUS.Models.ViewModels
{
    public class EditUserVM : Users
    {
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
