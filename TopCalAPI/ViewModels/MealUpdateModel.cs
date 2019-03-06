using System;

namespace TopCalAPI.ViewModels
{
    public class MealUpdateModel
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public float NumberOfCalories { get; set; }

        public string Description { get; set; }

        public TimeSpan Time { get; set; }
    }
}
