using System;
using System.Collections.Generic;
using TopCal.Data.Model;

namespace TopCal.Data.MockDb
{
    public static class DbTestData
    {
        private static Meal meal1 = new Meal
        {
            DateTime = DateTime.Now,
            Name = "Meal 1 Test",
            NoCalories = 2500
        };

        private static Meal meal2 = new Meal
        {
            DateTime = DateTime.Now.AddDays(-10),
            Name = "Meal 2 Test",
            NoCalories = 1500
        };

        private static Meal meal3 = new Meal
        {
            DateTime = DateTime.Now.AddYears(-2),
            Name = "Meal 3 Test",
            NoCalories = 4500
        };
        private static Meal meal4 = new Meal
        {
            DateTime = DateTime.Now.AddMonths(2),
            Name = "Meal 4 Test",
            NoCalories = 2500
        };

        private static Meal meal5 = new Meal
        {
            DateTime = DateTime.Now,
            Name = "Meal 5 Test",
            NoCalories = 500
        };

        public static List<Meal> Meals = new List<Meal> {meal1, meal2, meal3, meal4, meal5};
    }
}
