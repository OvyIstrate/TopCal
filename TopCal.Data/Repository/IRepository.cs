using System.Threading.Tasks;

namespace TopCal.Data.Repository
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAllAsync();
    }
}
