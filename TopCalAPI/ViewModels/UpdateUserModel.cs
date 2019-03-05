using System.ComponentModel.DataAnnotations;
using TopCal.Data.Enums;

namespace TopCalAPI.ViewModels
{
    public class UpdateUserModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public float CaloriesTarget { get; set; }

        public RoleEnum Role { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
