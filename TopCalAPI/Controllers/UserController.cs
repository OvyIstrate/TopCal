using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TopCalAPI.Filters;
using TopCalAPI.Services.Interfaces;
using TopCalAPI.ViewModels;

namespace TopCalAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private const int FAILED_CODE = 500;

        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET api/values
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

                var response = new ResponseModel
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

        // PUT api/values/5
        [HttpPut("{id}")]
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

                var response = new ResponseModel
                {
                    Message = "You've successfully updated an user"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when updating an user {ex.Message}");
            }

            return BadRequest(_userService.GetErrors());
        }

        // DELETE api/values/5
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

                var response = new ResponseModel
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
