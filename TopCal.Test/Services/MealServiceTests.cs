using TopCalAPI.Services;
using Xunit;

namespace TopCal.Test.Services
{
    public class MealServiceTests
    {
        private readonly MealService _mealService;

        public MealServiceTests()
        {
            _mealService = new MealService();
        }

        [Fact]
        public void GetMealsByName_ReturnsOne()
        {
            var result = _mealService.GetMeals("1");

            Assert.Equal(1, result.Count);
        }
    }
}
 