using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopCal.Data.Entities;
using TopCal.Data.Repository;
using TopCalAPI.Filters;
using TopCalAPI.Services.Interfaces;
using TopCalAPI.ViewModels;

namespace TopCalAPI.Services.Implementation
{
    public class MealService : IMealService
    {
        private readonly IRepository _repository;
        private readonly IFilter<Meal, MealFilterModel> _filter;

        public MealService(IRepository repository,
                           IFilter<Meal, MealFilterModel> filter)
        {
            _repository = repository;
            _filter = filter;
        }

        public async Task<List<Meal>> GetMealsAsync(string userId, MealFilterModel filterModel)
        {
            var query = _repository.GetAll<Meal>().Where(x => x.UserId == userId);

            var result =  await _filter.Filter(filterModel, query).ToListAsync();

            return result;
        }

        public async Task<Meal> GetMealAsync(string userId, Guid id)
        {
            return await _repository.GetAll<Meal>().Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddMealAsync(MealCreateModel model)
        {
            if (model != null)
            {
                var meal = new Meal
                {
                    Date = model.Date,
                    Description = model.Description,
                    Id = Guid.NewGuid(),
                    NumberOfCalories = model.NumberOfCalories,
                    Time = model.Time,
                    UserId = model.UserId
                };

                _repository.Add(meal);

                await _repository.SaveAllAsync();
            }
        }

        public async Task UpdateMealAsync(MealUpdateModel model)
        {
            if (model != null)
            {
                var entity = await _repository.GetAll<Meal>().FirstOrDefaultAsync(x => x.Id == model.Id);

                if (entity == null)
                {
                    throw new Exception("Meal entity is null on update operation!");
                }

                entity.Date = model.Date;
                entity.Time = model.Time;
                entity.NumberOfCalories = model.NumberOfCalories;
                entity.Description = model.Description;

                _repository.Update(entity);

                await _repository.SaveAllAsync();
            }
        }

        public async Task DeleteMealAsync(Guid id)
        {
            var entity = await _repository.GetAll<Meal>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                _repository.Delete(entity);

                await _repository.SaveAllAsync();
            }
        }
    }
}
