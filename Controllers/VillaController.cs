using MagicVilla_API.Data;
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
        private readonly ApplicationDbContext _db;

        public VillaController(
            ILogger<VillaController> logger,
            ApplicationDbContext db
        ) 
        {
            _logger= logger;
            _db= db;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Getting all villas");
            return Ok(_db.Villas.ToList());
        }


        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0) return BadRequest("id 0 is not allow¡");
     
            var villaFounded = _db.Villas.FirstOrDefault(x => x.Id == id);  
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
            if(_db.Villas.FirstOrDefault(v => v.Name.ToLower() == villa.Name.ToLower()) != null)
            {
                _logger.LogError("Name villa had already created¡");
                ModelState.AddModelError("NameExisted", "Name villa had already created¡");
                return BadRequest(ModelState);
            }

            Villa newVilla = new()
            {
                Name = villa.Name,
                Description = villa.Description,
                ImageUrl= villa.ImageUrl,
                Price= villa.Price,
                Area = villa.Area
            };

            _db.Villas.Add(newVilla);
            _db.SaveChanges();
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
            
            var villaFounded = _db.Villas.FirstOrDefault(v => v.Id == id);
            if (villaFounded == null) return NotFound();
            
            _db.Villas.Remove(villaFounded);
            _db.SaveChanges();
            return NoContent();
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> UpdateVilla(int id, [FromBody] UpdateVillaDTO villa)
        {
            if (villa == null) return BadRequest();
 
            var villaFounded = _db.Villas.FirstOrDefault(v => v.Id == id);
            if (villaFounded == null) return NotFound();

            Villa newVilla = new()
            {
                Name = villa.Name,
                Description = villa.Description,
                ImageUrl = villa.ImageUrl,
                Price = villa.Price,
                Area = villa.Area
            };

            _db.Update(newVilla);
            _db.SaveChanges();
            return Ok(newVilla);
        }


        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> villa)
        {
            if (villa == null) return BadRequest();
            var villaFounded = _db.Villas.FirstOrDefault(v => v.Id == id);
            if (villaFounded == null) return NotFound();

            VillaDTO newVilla = new()
            {
                Id= id,
                Name = villaFounded.Name,
                Description = villaFounded.Description,
                ImageUrl = villaFounded.ImageUrl,
                Price = villaFounded.Price,
                Area = villaFounded.Area
            };

            villa.ApplyTo(newVilla, ModelState);
            if (!ModelState.IsValid) return BadRequest();

            Villa villaModel = new()
            {
                Id = newVilla.Id,
                Name = newVilla.Name,
                Description = newVilla.Description,
                ImageUrl = newVilla.ImageUrl,
                Price = newVilla.Price,
                Area = newVilla.Area
            };

            _db.Villas.Update(villaModel);
            _db.SaveChanges();

            return Ok(villaFounded);
        }
    }
}
