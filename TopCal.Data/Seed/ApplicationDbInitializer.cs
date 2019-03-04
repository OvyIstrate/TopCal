using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;

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
            if (_context.Users.Any())
            {
                //TODO Prepare Some Seed data for the meals
                await _context.SaveChangesAsync();
            }
        }
    }
}
