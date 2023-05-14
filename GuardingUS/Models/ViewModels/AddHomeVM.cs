using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuardingUS.Models.ViewModels
{
    public class AddHomeVM : Homes
    {
        public IEnumerable<SelectListItem> Address {get; set;}

        public IEnumerable<SelectListItem> Owners { get; set; }
    }
}
