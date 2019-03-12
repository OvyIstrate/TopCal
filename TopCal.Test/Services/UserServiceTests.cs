using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using TopCal.Data.Entities;
using TopCal.Data.Repository;
using TopCalAPI.Config;
using TopCalAPI.Services.Implementation;
using TopCalAPI.Services.Interfaces;

namespace TopCal.Test.Services
{
    public class UserServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<IRepository> _repository;
        private readonly Mock<IPasswordHasher<ApplicationUser>> _hasher;
        private readonly Mock<IEmailService> _emailService;
        private readonly Mock<IOptions<AppSettings>> _appSettings;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userManager = new Mock<UserManager<ApplicationUser>>();
            _repository = new Mock<IRepository>();
            _hasher = new Mock<IPasswordHasher<ApplicationUser>>();
           _emailService = new Mock<IEmailService>();
            _appSettings = new Mock<IOptions<AppSettings>>();


            _userService = new UserService(_userManager.Object, _repository.Object, _appSettings.Object, _hasher.Object,
                _emailService.Object);
        }
    }
}
