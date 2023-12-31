﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.DTOs
{
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
        public string ImageUrl { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Area { get; set; }
    }
}
