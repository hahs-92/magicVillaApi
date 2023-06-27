using AutoMapper;
using MagicVilla_API.DTOs;
using MagicVilla_API.Models;

namespace MagicVilla_API
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();

            // lo mismo que arriba, pero en una sola linea
            CreateMap<Villa, CreateVillaDTO>().ReverseMap();
            CreateMap<Villa, UpdateVillaDTO>().ReverseMap();
        }
    }
}
