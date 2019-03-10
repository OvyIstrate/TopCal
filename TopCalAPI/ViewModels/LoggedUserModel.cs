using TopCal.Data.Enums;

namespace TopCalAPI.ViewModels
{
    public class LoggedUserModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Token { get; set; }

        public RoleEnum Role { get; set; }
    }
}
