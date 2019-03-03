using System;
using Microsoft.EntityFrameworkCore;
using TopCal.Data.Model;

namespace TopCal.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Meal> Meals { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
