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
            await _mealService.AddMealAsync(model);

            return Ok($"The {model.Description} has been added!");
        }

        // PUT: api/Meal/5
        [HttpPut("{id}")]
        [ValidateModel]
        [Authorize(Policy = "MealManagers")]
        public async Task<ActionResult> Put(Guid id, [FromBody] MealUpdateModel model)
        {
            model.Id = id;
            await _mealService.UpdateMealAsync(model);

            return Ok($"The {model.Id} has been updated");
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "MealManagers")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mealService.DeleteMealAsync(id);

            return Ok($"The {id} meal has been deleted!");
        }
    }
}
