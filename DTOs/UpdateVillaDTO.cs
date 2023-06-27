using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.DTOs
{
    public class UpdateVillaDTO
    {
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public int Area { get; set; }
    }
}
