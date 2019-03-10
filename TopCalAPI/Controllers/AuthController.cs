using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopCalAPI.Filters;
using TopCalAPI.Services.Interfaces;
using TopCalAPI.ViewModels;

namespace TopCalAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const int FAILED_CODE = 418;

        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService,
                              ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "UserManagers")]
        public async Task<ActionResult> GetUsers()
        {
            var users = await _userService.GetAll(User.Identity.Name);

            return Ok(users);
        }

        [HttpGet("{userId}")]
        [Authorize(Policy = "UserManagers")]
        public async Task<ActionResult> Get(string userId)
        {
            var user = await _userService.Get(userId);

            return Ok(user);
        }

        [HttpPost("create")]
        [ValidateModel]
        [Authorize(Policy = "UserManagers")]
        public async Task<IActionResult> Post([FromBody] CreateUserModel model)
        {
            try
            {
                var result = await _userService.AddUser(model);

                if (!result)
                {
                    var errors = _userService.GetErrors();
                    return OperationFailed(errors.ToList());
                }

                var response = new SuccessResponseModel
                {
                    Message = "You've successfully created an user",
                    Success = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when creating an user {ex.Message}");
            }

            return BadRequest(_userService.GetErrors());

        }

        [HttpDelete("{userId}")]
        [Authorize(Policy = "UserManagers")]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                var deleteResult = await _userService.DeleteUser(userId);

                if (!deleteResult)
                {
                    return OperationFailed(_userService.GetErrors());
                }

                var response = new SuccessResponseModel
                {
                    Message = "You've successfully deleted the user",
                    Success = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on deleting the user {ex.Message}");
            }

            return BadRequest(_userService.GetErrors());
        }

        [HttpPut]
        [ValidateModel]
        [Authorize(Policy = "UserManagers")]
        public async Task<IActionResult> Put([FromBody] UpdateUserModel model)
        {
            try
            {
                var updateResult = await _userService.UpdateUser(model);

                if (!updateResult)
                {
                    return OperationFailed(_userService.GetErrors());
                }

                var response = new SuccessResponseModel
                {
                    Message = "You've successfully updated an user"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when creating an user {ex.Message}");
            }

            return BadRequest(_userService.GetErrors());
        }

        // POST: api/user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var result = await _userService.Register(model);

                if (!result)
                {
                    return OperationFailed(_userService.GetErrors());
                }

                var response = new SuccessResponseModel
                {
                    Message = "You've Successfully registered!",
                    Success =  true
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on user registry: {ex.Message}");
            }

            return BadRequest(_userService.GetErrors());
        }

        //[ValidateModel]
        [HttpPost("token")]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var result = await _userService.CreateToken(model);

                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while creating JWT: {ex}");
            }

            return BadRequest(_userService.GetErrors());
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
    }
}
