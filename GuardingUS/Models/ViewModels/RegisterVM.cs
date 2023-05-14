using System.ComponentModel.DataAnnotations;

namespace GuardingUS.Models.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "This field {0} is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "This field {0} is required")]
        [EmailAddress(ErrorMessage = "This field should be a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field {0} is required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        //public int IdRole { get; set; }

        [Required(ErrorMessage = "This field {0} is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}
