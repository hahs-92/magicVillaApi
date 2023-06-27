using MagicVilla_API.Controllers;
using MagicVilla_API.Data;
using MagicVilla_API.Models;

namespace MagicVilla_API.Repositories
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
            
        }

        public async Task<Villa> Update(Villa villa)
        {
            villa.UpdatedAt = DateTime.Now;
            _db.Villas.Update(villa);
            await _db.SaveChangesAsync();
            return villa;
        }
    }
}
