using System;

namespace TopCalAPI.ViewModels
{
    public class MealFilterModel : BaseFilterModel
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public TimeSpan? FromTime { get; set; }

        public TimeSpan? ToTime { get; set; }
    }
}
