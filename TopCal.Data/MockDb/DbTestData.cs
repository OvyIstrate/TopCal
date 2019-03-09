using System;
using System.Collections.Generic;
using TopCal.Data.Entities;

namespace TopCal.Data.MockDb
{
    public static class DbTestData
    {
        private static Meal meal1 = new Meal
        {
            Date = DateTime.Now,
            Description = "Meal 1 Test",
            NumberOfCalories = 2500
        };

        private static Meal meal2 = new Meal
        {
            Date = DateTime.Now.AddDays(-10),
            Description = "Meal 2 Test",
            NumberOfCalories = 1500
        };

        private static Meal meal3 = new Meal
        {
            Date = DateTime.Now.AddYears(-2),
            Description = "Meal 3 Test",
            NumberOfCalories = 4500
        };
        private static Meal meal4 = new Meal
        {
            Date = DateTime.Now.AddMonths(2),
            Description = "Meal 4 Test",
            NumberOfCalories = 2500
        };

        private static Meal meal5 = new Meal
        {
            Date = DateTime.Now,
            Description = "Meal 5 Test",
            NumberOfCalories = 500
        };

        public static List<Meal> Meals = new List<Meal> {meal1, meal2, meal3, meal4, meal5};
    }
}
