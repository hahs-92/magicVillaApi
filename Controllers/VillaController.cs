using MagicVilla_API.DTOs;
using MagicVilla_API.FakeData;
using MagicVilla_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villas);
        }

        [HttpGet("id")]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0) return BadRequest("id 0 is not allow¡");
            var villaFounded = VillaStore.villas.FirstOrDefault(v => v.Id == id);
            if (villaFounded == null) return NotFound("Villa not Found¡");      
            return Ok(villaFounded);
        }
    }
}
