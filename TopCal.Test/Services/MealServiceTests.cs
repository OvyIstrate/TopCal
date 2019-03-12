using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            _repository = new Mock<IRepository>();
            _filter = new Mock<IFilter<Meal, MealFilterModel>>();
            _mealService = new MealService(_repository.Object, _filter.Object);
        }

    }
}
 