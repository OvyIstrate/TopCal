using Microsoft.EntityFrameworkCore;
using Moq;
using TopCal.Data;
using TopCal.Data.Entities;
using Xunit;

namespace TopCal.Test.Repository
{
    public class RepositoryTests
    {
        private readonly Data.Repository.Repository _repository;
        private readonly Mock<ApplicationDbContext> _dbContext;
        private readonly Mock<DbSet<Meal>> _meals;

        public RepositoryTests()
        {
            _dbContext = new Mock<ApplicationDbContext>();
            _meals = new Mock<DbSet<Meal>>();
            _repository = new Data.Repository.Repository(_dbContext.Object);    
        }

        [Fact]
        public void GetMeals_ReturnsNone()
        {
            var meals =  _repository.GetAll<Meal>();

            Assert.Empty(meals);
        }
    }
}
