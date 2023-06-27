using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.DTOs
{
    public class VillaDTO
    {
        public int Id { get; set; }
        //Data Annotation
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
