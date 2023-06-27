using MagicVilla_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
       

        // de esta manera expeficamos que se va a crear una tabla
        // en nuestra DB basado en este modelo
        public DbSet<Villa> Villas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=MagicVilla;Username=postgres;Password=TravelMate2420");

    }
}
