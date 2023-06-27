using MagicVilla_API.Models;

namespace MagicVilla_API.Repositories
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> Update(Villa villa);
    }
}
