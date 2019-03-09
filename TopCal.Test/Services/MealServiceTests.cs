using System;
using System.Threading.Tasks;
using Moq;
using TopCal.Data.Entities;
using TopCal.Data.Repository;
using TopCalAPI.Filters;
using TopCalAPI.Services.Implementation;
using TopCalAPI.ViewModels;
using Xunit;

namespace TopCal.Test.Services
{
    public class MealServiceTests
    {
        private readonly Mock<IFilter<Meal, MealFilterModel>> _filter;
        private readonly Mock<IRepository> _repository;
        private readonly MealService _mealService;

        public MealServiceTests()
        {
            _mealService = new MealService(_repository.Object, _filter.Object);
        }

        [Fact]
        public async Task GetMealById_ReturnsEntry_Success()
        {
            var result =  await _mealService.GetMealAsync(Guid.NewGuid());

            Assert.NotNull(result);
        }
    }
}
 