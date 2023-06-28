using MagicVilla_API.Models;

namespace MagicVilla_API.Repositories
{
    public interface IVillaNumberRepository: IRepository<VillaNumber>
    {
        Task<VillaNumber> Update(VillaNumber entity);
    }
}
