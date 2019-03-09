using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using TopCal.Data.Entities;

namespace TopCal.Data.Seed
{
    public class ApplicationDbInitializer
    {
        private ApplicationDbContext _context;

        public ApplicationDbInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Seed()
        {
            if (!_context.Meals.Any())
            {
                _context.Meals.AddRange(GetSeedMeals());
                await _context.SaveChangesAsync();
            }
        }

        private static List<Meal> GetSeedMeals()
        {

            return new List<Meal>
            {
                new Meal
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "Breakfast Regular",
                    UserId = "477705e4-2932-4757-9ded-b398a8121285",
                    Id = Guid.NewGuid(),
                    Time = new TimeSpan(9, 30, 0),
                    NumberOfCalories = 1500
                },
                new Meal
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "Lunch Regular",
                    UserId = "477705e4-2932-4757-9ded-b398a8121285",
                    Id = Guid.NewGuid(),
                    Time = new TimeSpan(12, 30, 0),
                    NumberOfCalories = 2500

                },
                new Meal
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "Dinner Regular",
                    UserId = "477705e4-2932-4757-9ded-b398a8121285",
                    Id = Guid.NewGuid(),
                    Time = new TimeSpan(19, 30, 0),
                    NumberOfCalories = 2500

                },
                new Meal
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "Snack Regular",
                    UserId = "477705e4-2932-4757-9ded-b398a8121285",
                    Id = Guid.NewGuid(),
                    Time = new TimeSpan(16, 20, 0),
                    NumberOfCalories = 800
                },
                new Meal
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "Breakfast Admin",
                    UserId = "c78e1314-55a8-4b9f-9836-2f9e6ec7986e",
                    Id = Guid.NewGuid(),
                    Time = new TimeSpan(9, 30, 0),
                    NumberOfCalories = 800

                },
                new Meal
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "Lunch Admin",
                    UserId = "c78e1314-55a8-4b9f-9836-2f9e6ec7986e",
                    Id = Guid.NewGuid(),
                    Time = new TimeSpan(12, 30, 0),
                    NumberOfCalories = 800

                },
                new Meal
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "Dinner Admin",
                    UserId = "c78e1314-55a8-4b9f-9836-2f9e6ec7986e",
                    Id = Guid.NewGuid(),
                    Time = new TimeSpan(19, 30, 0),
                    NumberOfCalories = 800

                },
                new Meal
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "Snack Admin",
                    UserId = "c78e1314-55a8-4b9f-9836-2f9e6ec7986e",
                    Id = Guid.NewGuid(),
                    Time = new TimeSpan(16, 20, 0),
                    NumberOfCalories = 800

                },
            };
        }
    }
}
