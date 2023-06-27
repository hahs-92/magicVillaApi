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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villas);
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0) return BadRequest("id 0 is not allow¡");
            var villaFounded = VillaStore.villas.FirstOrDefault(v => v.Id == id);
            if (villaFounded == null) return NotFound("Villa not Found¡");      
            return Ok(villaFounded);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villa)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
          
            //custom validation
            if(VillaStore.villas.FirstOrDefault(v => v.Name.ToLower() == villa.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NameExisted", "Name villa had already created¡");
                return BadRequest(ModelState);
            }

            villa.Id = VillaStore.villas
                .OrderByDescending(v => v.Id)
                .FirstOrDefault()!.Id + 1;

            VillaStore.villas.Add(villa);
            return CreatedAtRoute("GetVilla", new {id=villa.Id}, villa);
        }
    }
}
