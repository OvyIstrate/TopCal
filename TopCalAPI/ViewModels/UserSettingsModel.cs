using System.ComponentModel.DataAnnotations;

namespace TopCalAPI.ViewModels
{
    public class UserSettingsModel
    {
        public string UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public float CaloriesTarget { get; set; }
    }
}
