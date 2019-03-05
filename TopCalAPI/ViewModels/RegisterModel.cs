using System.ComponentModel.DataAnnotations;

namespace TopCalAPI.ViewModels
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string  ConfirmPassword { get; set; }

    }
}
