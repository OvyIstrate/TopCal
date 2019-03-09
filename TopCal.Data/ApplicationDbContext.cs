using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TopCal.Data.Entities;

namespace TopCal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Meal> Meals { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ConvertProperties(builder);
        }


        private void ConvertProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meal>().Property(x => x.Time).HasConversion(TimeSpanConverter);
        }


        public static ValueConverter TimeSpanConverter { get; } = new ValueConverter<TimeSpan, long?>(
            v => v == null ? (long?) null : v.Ticks,
            v => v == null ? new TimeSpan() : new TimeSpan(v.Value));

    }
}
