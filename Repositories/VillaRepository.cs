using MagicVilla_API.Controllers;
using MagicVilla_API.Data;
using MagicVilla_API.Models;

namespace MagicVilla_API.Repositories
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<VillaRepository> _logger;

        public VillaRepository(ApplicationDbContext db, ILogger<VillaRepository> logger) : base(db)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Villa> Update(Villa villa)
        {
            try
            {
                villa.UpdatedAt = DateTime.Now;
                _db.Villas.Update(villa);
                await _db.SaveChangesAsync();
                return villa;
            } catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new Villa();
            }
        }
    }
}
