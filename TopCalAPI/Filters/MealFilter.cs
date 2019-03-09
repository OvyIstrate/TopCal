using System;
using System.Linq;
using TopCal.Data.Entities;
using TopCalAPI.ViewModels;

namespace TopCalAPI.Filters
{
    public class MealFilter : IFilter<Meal, MealFilterModel>
    {
        public IQueryable<Meal> Filter(MealFilterModel filterModel, IQueryable<Meal> query)
        {
            if (filterModel == null)
            {
                return query;
            }

            if (filterModel.FromDate.HasValue)
            {
                query = query.Where(x => DateTime.Compare(filterModel.FromDate.Value, x.Date) <= 0);
            }

            if (filterModel.ToDate.HasValue)
            {
                query = query.Where(x => DateTime.Compare(filterModel.ToDate.Value, x.Date) >= 0);
            }

            if (filterModel.FromTime.HasValue)
            {
                query = query.Where(x => x.Time.TotalSeconds >= filterModel.FromTime.Value.TotalSeconds);
            }

            if (filterModel.ToTime.HasValue)
            {
                query = query.Where(x => x.Time.TotalSeconds <= filterModel.ToTime.Value.TotalSeconds);
            }

            return query;
        }
    }
}
