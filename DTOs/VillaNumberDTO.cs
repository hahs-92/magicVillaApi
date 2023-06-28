using MagicVilla_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.DTOs
{
    public class VillaNumberDTO
    {
        public int VillaNo { get; set; }
        public int VillaId { get; set; }
        public Villa Villa { get; set; }
        public string Detail { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
