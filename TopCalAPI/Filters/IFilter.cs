using System.Linq;

namespace TopCalAPI.Filters
{
    public interface IFilter<T, U>
    {
        IQueryable<T> Filter(U filterModel, IQueryable<T> query);
    }
}
