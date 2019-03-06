using System;
using System.ComponentModel.DataAnnotations;

namespace TopCalAPI.ViewModels
{
    public class MealCreateModel
    {
        [Required]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public float NumberOfCalories { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        public string UserId { get; set; }
    }
}
