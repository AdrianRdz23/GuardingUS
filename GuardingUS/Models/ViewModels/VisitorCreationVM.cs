using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuardingUS.Models.ViewModels
{
    public class VisitorCreationVM : Visitors
    {
        //public Address IdAddress { get; set; }

        public IEnumerable<SelectListItem> Homes { get; set; }
        public IEnumerable<SelectListItem> Address { get; set; }

    }
}
