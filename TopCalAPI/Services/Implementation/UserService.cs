using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TopCal.Data.Constants;
using TopCal.Data.Entities;
using TopCal.Data.Enums;
using TopCal.Data.Repository;
using TopCalAPI.Config;
using TopCalAPI.Services.Interfaces;
using TopCalAPI.ViewModels;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace TopCalAPI.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository _repository;
        private readonly IPasswordHasher<ApplicationUser> _hasher;
        private readonly IEmailService _emailService;
        private readonly AppSettings _appSettings;
        private readonly List<string> _errors;

        public UserService(UserManager<ApplicationUser> userManager,
                           IRepository repository,
                           IOptions<AppSettings> appSettings,
                           IPasswordHasher<ApplicationUser> hasher, IEmailService emailService)
        {
            _userManager = userManager;
            _repository = repository;
            _appSettings = appSettings.Value;
            _hasher = hasher;
            _emailService = emailService;
            _errors = new List<string>();
        }

        public async Task<List<UserViewModel>> GetAll(string userId)
        {
            var users = await _repository.GetAll<ApplicationUser>().Where(x => x.Id != userId).ToListAsync();
            var result = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();

                result.Add(new UserViewModel
                {
                    Id = user.Id,
                    CaloriesTarget = user.CaloriesTarget,
                    FirstName = user.FirstName,
                    Email = user.Email,
                    LastName = user.LastName,
                    Role = role,
                    UserName = user.UserName
                });
            }

            return result;
        }

        public async Task<UserViewModel> Get(string userId)
        {
            var user = await _repository.GetAll<ApplicationUser>().FirstOrDefaultAsync(x => x.Id == userId);

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var result = new UserViewModel
            {
                Id = user.Id,
                CaloriesTarget = user.CaloriesTarget,
                FirstName = user.FirstName,
                Email = user.Email,
                LastName = user.LastName,
                Role = role,
                UserName = user.UserName
            };

            return result;
        }

        public async Task<bool> AddUser(CreateUserModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };

                var role = GetRoleName(model.Role);

                model.Password = PasswordHelper.GenerateRandomPassword();

                var createResult = await _userManager.CreateAsync(user, model.Password);
                var roleResult = await _userManager.AddToRoleAsync(user, role);
                var claimResult = await _userManager.AddClaimAsync(user, new Claim(role, "True"));

                if (!createResult.Succeeded || !claimResult.Succeeded || !roleResult.Succeeded)
                {
                    _errors.AddRange(createResult.Errors.Concat(claimResult.Errors).Concat(roleResult.Errors)
                        .Select(x => x.Description).ToList());
                }

                if (createResult.Succeeded)
                {
                    //Send Email
                    var mailInfo = new EmailInfo
                    {
                        TempPassword = model.Password,
                        UserEmail = model.Email,
                        Username = model.UserName
                    };

                    await _emailService.SendMailAsync(mailInfo);
                }

                return createResult.Succeeded && claimResult.Succeeded && roleResult.Succeeded;
            }

            _errors.Add("Bad Request: User already exists");
            return false;

        }

        public async Task<bool> UpdateUser(UpdateUserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.CaloriesTarget = model.CaloriesTarget;

                var role = GetRoleName(model.Role);
                var currentRole =  await _userManager.GetRolesAsync(user);

                if (!currentRole.First().Equals(role))
                {
                    await _userManager.RemoveFromRoleAsync(user, currentRole.First());
                    await _userManager.RemoveClaimAsync(user, new Claim(currentRole.First(), "True"));
                    await _userManager.AddToRoleAsync(user, role);
                    await _userManager.AddClaimAsync(user, new Claim(role, "True"));
                }
                
                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                   _errors.AddRange(updateResult.Errors.Select(x => x.Description).ToList());
                }

                return updateResult.Succeeded;
            }

            _errors.Add("Bad Request: User doesn't exist!");

            return false;
        }

        public async Task<bool> UpdateSettings(UserSettingsModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user != null)
            {
                user.CaloriesTarget = model.CaloriesTarget;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    _errors.AddRange(updateResult.Errors.Select(x => x.Description).ToList());
                }

                return updateResult.Succeeded;
            }

            _errors.Add("Bad Request: User doesn't exist!");

            return false;
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var deleteResult = await _userManager.DeleteAsync(user);

                if (!deleteResult.Succeeded)
                {
                    _errors.AddRange(deleteResult.Errors.Select(x => x.Description).ToList());
                }

                return deleteResult.Succeeded;
            }

            _errors.Add("Bad Request: User doesn't exist");
            return false;

        }

        public async Task<bool> Register(RegisterModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var role = GetRoleName(RoleEnum.Regular);

                var createResult = await _userManager.CreateAsync(user, model.Password);
                var roleResult = await _userManager.AddToRoleAsync(user, role);
                var claimResult = await _userManager.AddClaimAsync(user, new Claim("Regular", "True"));

                if (!createResult.Succeeded || !claimResult.Succeeded || !roleResult.Succeeded)
                {
                    _errors.AddRange(createResult.Errors.Concat(claimResult.Errors).Select(x => x.Description).ToList());
                }

                return createResult.Succeeded && claimResult.Succeeded && roleResult.Succeeded;
            }

            _errors.Add("Bad Request: User already exists!");

            return false;
        }

        public async Task<LoggedUserModel> CreateToken(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                if (_hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                {
                    var userClaims = await _userManager.GetClaimsAsync(user);

                    var roleClaim = userClaims.First();

                    var token = GenerateJwtToken(user, userClaims);

                    var result = new LoggedUserModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Token = token,
                        Role =  Enum.Parse<RoleEnum>(roleClaim.Type),
                        CaloriesTarget = user.CaloriesTarget
                    };

                    return result;
                }
            }
            _errors.Add("Bad Request: Login failed! User is not available!");
            return null;
        }

        public List<string> GetErrors()
        {
            return _errors;
        }

        private string GetRoleName(RoleEnum modelRole)
        {
            switch (modelRole)
            {
                case RoleEnum.Regular: return NameConstants.RegularRole;
                case RoleEnum.Manager: return NameConstants.ManagerRole;
                case RoleEnum.Admin: return NameConstants.AdminRole;
                default: return NameConstants.RegularRole;
            }
        }

        private string GenerateJwtToken(ApplicationUser user, IList<Claim> userClaims)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }.Union(userClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Token.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(30);

            var token = new JwtSecurityToken(
                _appSettings.Token.Issuer,
                _appSettings.Token.Audience,
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
