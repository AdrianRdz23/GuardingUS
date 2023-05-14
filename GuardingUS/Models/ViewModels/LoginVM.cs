using System.ComponentModel.DataAnnotations;

namespace GuardingUS.Models.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "The field {0} is required")]
        //[EmailAddress(ErrorMessage = "This field should be a valid email")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "This field {0} is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
