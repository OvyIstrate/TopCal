using System.ComponentModel.DataAnnotations;
using TopCal.Data.Enums;

namespace TopCalAPI.ViewModels
{
    public class CreateUserModel
    {
        [Required]
        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public RoleEnum Role { get; set; }
        
    }
}
