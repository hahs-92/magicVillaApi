using System.Linq.Expressions;

namespace MagicVilla_API.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entidad);
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tracked= true);
        Task Remove(T entidad);
        Task Save();
    }
}
