using System.Collections.Generic;
using System.Threading.Tasks;
using TopCalAPI.ViewModels;

namespace TopCalAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserViewModel>> GetAll(string userId);

        Task<bool> AddUser(CreateUserModel model);

        Task<bool> UpdateUser(UpdateUserModel model);

        Task<bool> DeleteUser(string userId);

        Task<bool> Register(RegisterModel model);

        Task<string> CreateToken(LoginModel model);

        List<string> GetErrors();
    }
}
