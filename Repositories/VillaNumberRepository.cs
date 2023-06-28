using MagicVilla_API.Data;
using MagicVilla_API.Models;

namespace MagicVilla_API.Repositories
{
    public class VillaNumberRepository: Repository<VillaNumber> , IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }

        public async Task<VillaNumber> Update(VillaNumber villa)
        {
            villa.UpdatedAt = DateTime.Now;
            _db.VillaNumbers.Update(villa);
            await _db.SaveChangesAsync();
            return villa;
        }
    }
}
