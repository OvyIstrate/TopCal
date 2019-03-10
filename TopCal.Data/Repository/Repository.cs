using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TopCal.Data.Entities;

namespace TopCal.Data.Repository
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return _context.Set<T>().AsQueryable();
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Set<T>().Attach(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
