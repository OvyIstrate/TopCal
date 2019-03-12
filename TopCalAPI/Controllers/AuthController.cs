using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        private const int FAILED_CODE = 500;

        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService,
                              ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }     

        [HttpPut("settings")]
        [ValidateModel]
        [Authorize]
        public async Task<ActionResult> UpdateSettings([FromBody] UserSettingsModel model)
        {
            try
            {
                model.UserId = User.Identity.Name;
                var result = await _userService.UpdateSettings(model);

                if (!result)
                {
                    return OperationFailed(_userService.GetErrors());
                }

                var response = new ResponseModel
                {
                    Message = "You've successfully updated your user settings"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when updating user settings {ex.Message}");
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

                var response = new ResponseModel
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
