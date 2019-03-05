using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TopCal.Data.Constants;
using TopCal.Data.Entities;
using TopCal.Data.Enums;
using TopCalAPI.Config;
using TopCalAPI.Filters;
using TopCalAPI.ViewModels;

namespace TopCalAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const int FAILED_CODE = 418;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthController> _logger;
        private IPasswordHasher<ApplicationUser> _hasher;
        private readonly AppSettings _appSettings;

        public AuthController(UserManager<ApplicationUser> userManager,
                              ILogger<AuthController> logger,
                              IPasswordHasher<ApplicationUser> hasher,
                              IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _logger = logger;
            _hasher = hasher;
            _appSettings = appSettings.Value;
        }

        [HttpPost("create")]
        [ValidateModel]
        [Authorize(Policy = "UserManagers")]
        public async Task<IActionResult> Post([FromBody] CreateUserModel model)
        {
            try
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

                    //TODO - Change Password Logic (Generate a password and inform the user on email that he has to change it)
                    model.Password = "DefaultP@ssw0rd!";

                    var role = GetRoleName(model.Role);

                    var createResult = await _userManager.CreateAsync(user, model.Password);
                    var roleResult = await _userManager.AddToRoleAsync(user, role);
                    var claimResult = await _userManager.AddClaimAsync(user, new Claim(role, "True"));

                    if (!createResult.Succeeded || !claimResult.Succeeded || !roleResult.Succeeded)
                    {
                        var errors = createResult.Errors.Concat(claimResult.Errors).Concat(roleResult.Errors);
                        return OperationFailed(errors.Select(x => x.Description).ToList());
                    }

                    return Ok("You've successfully created an user");

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when creating an user {ex.Message}");
            }

            return BadRequest($"User {model.UserName} already exists in the system!");

        }

        [Authorize(Policy = "UserManagers")]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    var deleteResult = await _userManager.DeleteAsync(user);

                    if (!deleteResult.Succeeded)
                    {
                        return OperationFailed(deleteResult.Errors.Select(x => x.Description).ToList());
                    }

                    return Ok($"You've successfully deleted the user: {user.UserName}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on deleting the user {ex.Message}");
            }

            return BadRequest($"The user with the Id: {userId} doesn't exist in the system!");
        }

        [HttpPut]
        [ValidateModel]
        [Authorize(Policy = "UserManagers")]
        public async Task<IActionResult> Put([FromBody] UpdateUserModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                var errors = new List<string>();

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.CaloriesTarget = model.CaloriesTarget;

                    var role = GetRoleName(model.Role);

                    var claims = await _userManager.GetClaimsAsync(user);

                    //TODO - Change Role Management
                    //var roleClaim = claims.FirstOrDefault(x => x.Type == role);

                    //if (roleClaim == null)
                    //{
                    //    var previousRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                        
                    //    var claimResult = await _userManager.RemoveClaimAsync(user, new Claim(previousRole, "True"));

                    //    if (!claimResult.Succeeded)
                    //    {
                    //        errors.AddRange(claimResult.Errors.Select(x => x.Description));
                    //    }
                    //}
                    //var roleResult = await _userManager.AddToRoleAsync(user, role);

                    var updateResult = await _userManager.UpdateAsync(user);

                    if (!updateResult.Succeeded)
                    {
                        return OperationFailed(updateResult.Errors.Select(x => x.Description).ToList());
                    }

                    return Ok("You've successfully updated an user");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when creating an user {ex.Message}");
            }

            return BadRequest($"User with Id:{model.UserId} doesn't exist!");
        }

        // POST: api/user
        [ValidateModel]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.UserName,
                        Email = model.Email
                    };

                    var createResult = await _userManager.CreateAsync(user, model.Password);
                    var claimResult = await _userManager.AddClaimAsync(user, new Claim("Regular", "True"));

                    if (!createResult.Succeeded || !claimResult.Succeeded)
                    {
                        var errors = createResult.Errors.Concat(claimResult.Errors);
                        return OperationFailed(errors.Select(x => x.Description).ToList());
                    }

                    return Ok("You've Successfully registered!");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on user registry: {ex.Message}");
            }

            return BadRequest($"User {model.UserName} already exists in the system!");
        }

        [ValidateModel]
        [HttpPost("token")]
        public async Task<IActionResult> CreateToken([FromBody] LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    if (_hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                    {
                        var userClaims = await _userManager.GetClaimsAsync(user);

                        var token = GenerateJwtToken(user, userClaims);

                        return Ok(token);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while creating JWT: {ex}");
            }

            return BadRequest("Failed to generate token");
        }

        private string GenerateJwtToken(ApplicationUser user, IList<Claim> userClaims)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
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

        private ObjectResult OperationFailed(List<string> errors)
        {
            return new ObjectResult(
                new ErrorResponseModel
                {
                    Errors = errors
                })
            {
                StatusCode = FAILED_CODE
            };
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
    }
}
