using System.Linq;
using System.Threading.Tasks;

namespace TopCal.Data.Repository
{
    public interface IRepository
    {
        IQueryable<T> GetAll<T>() where T : class;

        void Add<T>(T entity) where T : class;

        void Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAllAsync();
    }
}
