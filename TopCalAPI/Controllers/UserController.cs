using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TopCalAPI.UserManagement.ViewModels;

namespace TopCalAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private const int SUCCESS_CODE = 200;
        private const int FAILED_CODE = 418;
        private const int ERROR_CODE = 500;
        private const int REDIRECT_CODE = 301;
        private const int BAD_REQUEST_CODE = 400;
        private const int UNAUTHORIZED_CODE = 401;

        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/user
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/user/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/user
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    user = new IdentityUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.UserName,
                        Email = model.Email
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                    {
                        return OperationFailed(result.Errors.Select(x => x.Description).ToList());
                    }

                    return Ok(new LoginSuccessResponseModel
                    {
                        Message = "You've Successfully registered"
                    });
                }
            }

            return BadRequest();
        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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
