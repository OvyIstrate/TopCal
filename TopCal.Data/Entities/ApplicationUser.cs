using Microsoft.AspNetCore.Identity;

namespace TopCal.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public float CaloriesTarget { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
