using MagicVilla_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            // soluciona un error con las fechas
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
       

        // de esta manera expeficamos que se va a crear una tabla
        // en nuestra DB basado en este modelo
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=MagicVilla;Username=postgres;Password=TravelMate2420");

        // nos permite crear datos por default
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
               new Villa()
               {
                   Id = 1,
                   Name = "villa test",
                   Description = "Description test",
                   ImageUrl = "https://image.com",
                   Price = 999,
                   Area = 60,
                   CreatedAt = DateTime.Now,
                   UpdatedAt = DateTime.Now
               },
               new Villa()
               {
                   Id = 2,
                   Name = "villa test 02",
                   Description = "Description test 02",
                   ImageUrl = "https://image02.com",
                   Price = 990,
                   Area = 40,
                   CreatedAt = DateTime.Now,
                   UpdatedAt = DateTime.Now
               });
        }

    }
}
