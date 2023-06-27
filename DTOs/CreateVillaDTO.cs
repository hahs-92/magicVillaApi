using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.DTOs
{
    public class VillaDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Area { get; set; }
    }
    public class CreateVillaDTO
    {
        //public int Id { get; set; }
        //Data Annotation
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Description { get; set; }
        [Required]
        public int Area { get; set; }
    }

    public class UpdateVillaDTO
    {
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        public int Area { get; set; }
    }
}
