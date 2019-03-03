using System.Collections.Generic;
using System.Linq;
using TopCal.Data.MockDb;
using TopCal.Data.Model;

namespace TopCalAPI.Services
{
    public class MealService
    {
        public List<Meal> GetMeals(string description)
        {
            return DbTestData.Meals.Where(x => x.Description.Contains(description)).ToList();
        }
    }
}
