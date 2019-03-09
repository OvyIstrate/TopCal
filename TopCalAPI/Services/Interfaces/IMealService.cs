using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TopCal.Data.Entities;
using TopCalAPI.ViewModels;

namespace TopCalAPI.Services.Interfaces
{
    public interface IMealService
    {
        Task<List<Meal>> GetMealsAsync(string userId, MealFilterModel model = null);
        Task<Meal> GetMealAsync(Guid id);
        Task AddMealAsync(MealCreateModel model);
        Task UpdateMealAsync(MealUpdateModel model);
        Task DeleteMealAsync(Guid mealId);
    }
}
