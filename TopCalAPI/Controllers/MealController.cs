using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopCalAPI.Filters;
using TopCalAPI.Services.Interfaces;
using TopCalAPI.ViewModels;

namespace TopCalAPI.Controllers
{
    [Route("api/meal")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IMealService _mealService;

        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }

        // GET: api/Meal
        [HttpGet]
        [Authorize(Policy = "MealManagers")]
        public async Task<IActionResult> Get()
        {
            var meals = await _mealService.GetMealsAsync(User.Identity.Name);

            return Ok(meals);
        }

        [ValidateModel]
        [HttpPost("search")]
        [Authorize(Policy = "MealManagers")]
        public async Task<IActionResult> Search([FromBody] MealFilterModel filterModel)
        {
            var meals = await _mealService.GetMealsAsync(User.Identity.Name, filterModel);

            return Ok(meals);
        }

        // GET: api/Meal/5
        [HttpGet("{id}", Name = "Get")]
        [Authorize(Policy = "MealManagers")]
        public async Task<IActionResult> Get(Guid id)
        {
            var meal = await _mealService.GetMealAsync(User.Identity.Name, id);

            return Ok(meal);
        }
         
        // POST: api/Meal
        [HttpPost]
        [ValidateModel]
        [Authorize(Policy = "MealManagers")]
        public async Task<ActionResult> Post([FromBody] MealCreateModel model)
        {
            model.UserId = User.Identity.Name;

            await _mealService.AddMealAsync(model);

            var response = new ResponseModel
            {
                Message = $"The {model.Description} has been added!"
            };

            return Ok(response);
        }

        // PUT: api/Meal/5
        [HttpPut]
        [ValidateModel]
        [Authorize(Policy = "MealManagers")]
        public async Task<ActionResult> Put([FromBody] MealUpdateModel model)
        {
            await _mealService.UpdateMealAsync(model);

            var response = new ResponseModel
            {
                Message = $"The meal has been updated"
            };

            return Ok(response);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "MealManagers")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mealService.DeleteMealAsync(id);

            var response = new ResponseModel
            {
                Message = "The meal has been deleted!"
            };

            return Ok(response);
        }
    }
}
