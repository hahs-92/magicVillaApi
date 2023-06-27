using MagicVilla_API.DTOs;

namespace MagicVilla_API.FakeData
{
    public static class VillaStore
    {
        public static List<VillaDTO> villas = new List<VillaDTO>
        {
            new VillaDTO { Id=1, Name="Villa 1",Description="xxxxx", Area=50},
            new VillaDTO { Id=2, Name="Villa 2",Description="xxxxx", Area=52},
            new VillaDTO { Id=3, Name="Villa 3",Description="xxxxx", Area=18},
            new VillaDTO { Id=4, Name="Villa 4",Description="xxxxx", Area=90},
            new VillaDTO { Id=5, Name="Villa 5",Description="xxxxx", Area=51},
        };
    }
}
