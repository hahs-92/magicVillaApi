using MagicVilla_API.Models;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.DTOs
{
    public class UpdateVillaNumberDTO
    {
        [Required]
        public int VillaId { get; set; }
        [Required]
        public string Detail { get; set; }
    }
}
