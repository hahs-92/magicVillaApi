using MagicVilla_API.DTOs;
using MagicVilla_API.FakeData;
using MagicVilla_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;

        public VillaController(ILogger<VillaController> logger) {
            _logger= logger;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Getting all villas");
            return Ok(VillaStore.villas);
        }


        [HttpGet("{id:int}", Name = "GetVilla")]
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
        public ActionResult<VillaDTO> CreateVilla([FromBody] CreateVillaDTO villa)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
          
            //custom validation
            if(VillaStore.villas.FirstOrDefault(v => v.Name.ToLower() == villa.Name.ToLower()) != null)
            {
                _logger.LogError("Name villa had already created¡");
                ModelState.AddModelError("NameExisted", "Name villa had already created¡");
                return BadRequest(ModelState);
            }

            //villa.Id = VillaStore.villas
            //    .OrderByDescending(v => v.Id)
            //    .FirstOrDefault()!.Id + 1;

            VillaDTO newVilla = new VillaDTO
            {
                Id = VillaStore.villas.Count + 1,
                Name = villa.Name,
                Description = villa.Description,
                Area = villa.Area
            };

            VillaStore.villas.Add(newVilla);
            return CreatedAtRoute("GetVilla", new {id=newVilla.Id}, villa);
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // utilizamos IActionResult porque no necesitamos devolver
        // un modelo. Retornaremos un NoContent
        public IActionResult DeleteVilla(int id) 
        { 
            if(id == 0) return BadRequest(ModelState);
            var villaFounded = VillaStore.villas.FirstOrDefault(v =>v.Id == id);
            if (villaFounded == null) return NotFound();
            
            VillaStore.villas.Remove(villaFounded);
            return NoContent();
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> UpdateVilla(int id, [FromBody] UpdateVillaDTO villa)
        {
            if (villa == null) return BadRequest();
            var villaFounded = VillaStore.villas.FirstOrDefault(v => v.Id == id);
            if (villaFounded == null) return NotFound();

            villaFounded.Name= villa.Name;
            villaFounded.Description= villa.Description;
            villaFounded.Area= villa.Area;

            return Ok(villaFounded);
        }


        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> villa)
        {
            if (villa == null) return BadRequest();
            var villaFounded = VillaStore.villas.FirstOrDefault(v => v.Id == id);
            if (villaFounded == null) return NotFound();

            villa.ApplyTo(villaFounded, ModelState);
            if (!ModelState.IsValid) return BadRequest();

            return Ok(villaFounded);
        }
    }
}
