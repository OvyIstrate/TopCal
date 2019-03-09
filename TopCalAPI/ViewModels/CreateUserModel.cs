using System.ComponentModel.DataAnnotations;
using TopCal.Data.Enums;

namespace TopCalAPI.ViewModels
{
    public class CreateUserModel
    {
        [Required]
        public string UserName { get; set; }

        public string Password { get; set; }

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public RoleEnum Role { get; set; } = RoleEnum.Regular;

    }
}
