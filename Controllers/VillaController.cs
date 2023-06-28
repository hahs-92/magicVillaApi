using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.DTOs;
using MagicVilla_API.FakeData;
using MagicVilla_API.Models;
using MagicVilla_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        //private readonly ApplicationDbContext _db;
        private readonly IVillaRepository _repo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaController(
            ILogger<VillaController> logger,
            //ApplicationDbContext db,
            IVillaRepository repo,
            IMapper mapper
        )
        {
            _logger = logger;
            //_db= db;
            _repo = repo;
            _mapper = mapper;
            _response = new();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Getting all villas");
                IEnumerable<Villa> villaList = await _repo.GetAll();
                //IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();

                _response.Result = _mapper.Map<IEnumerable<VillaDTO>>(villaList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            } catch(Exception ex)
            {
                _logger.LogError("GetVillas" ,ex.Message);
                _response.IsSucceeded = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }
        }


        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villaFounded = await _repo.Get(x => x.Id == id);
                //var villaFounded = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
                if (villaFounded == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSucceeded = false;
                    return NotFound(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaDTO>(villaFounded);
                return Ok(_response);
            } catch(Exception ex)
            {
                _logger.LogError("GetVilla", ex.Message);
                _response.IsSucceeded = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] CreateVillaDTO createDTO)
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] CreateVillaDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                //custom validation
                //if (await _db.Villas.FirstOrDefaultAsync(v => v.Name.ToLower() == createDTO.Name.ToLower()) != null)
                if (await _repo.Get(v => v.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    _logger.LogError("Name villa had already created¡");
                    ModelState.AddModelError("NameExisted", "Name villa had already created¡");
                    return BadRequest(ModelState);
                }

                Villa newVilla = _mapper.Map<Villa>(createDTO);
                newVilla.CreatedAt= DateTime.Now;
                newVilla.UpdatedAt= DateTime.Now;

                //await _db.Villas.AddAsync(newVilla);
                //await _db.SaveChangesAsync();
                await _repo.Create(newVilla);
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = newVilla;
                return CreatedAtRoute("GetVilla", new { id = newVilla.Id }, _response);
            } catch(Exception ex)
            {
                _logger.LogError("CreateVilla", ex.Message);
                _response.IsSucceeded = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // utilizamos IActionResult porque no necesitamos devolver
        // un modelo. Retornaremos un NoContent
        // a las interfaces no s ele spuede colocar un Tipo
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSucceeded = false;
                    //BadRequest(ModelState); 
                    BadRequest(_response);
                }

                //var villaFounded = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
                var villaFounded = await _repo.Get(v => v.Id == id);
                if (villaFounded == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSucceeded = false;
                    return NotFound(_response);
                }

                //_db.Villas.Remove(villaFounded);
                //await _db.SaveChangesAsync();
                await _repo.Remove(villaFounded);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteVilla", ex.Message);
                _response.IsSucceeded = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<VillaDTO>> UpdateVilla(int id, [FromBody] UpdateVillaDTO updateVillaDTO)
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] UpdateVillaDTO updateVillaDTO)
        {
            try
            {
                if (updateVillaDTO == null) return BadRequest();
                if (!ModelState.IsValid) return BadRequest(ModelState);

                //var villaFounded = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
                var villaFounded = await _repo.Get(v => v.Id == id, tracked: false);
                if (villaFounded == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSucceeded = false;
                    return NotFound(_response);
                }

                Villa newVilla = _mapper.Map<Villa>(updateVillaDTO);
                newVilla.Id = id;
                newVilla.UpdatedAt= DateTime.Now;
                //_db.Update(newVilla);
                //await _db.SaveChangesAsync();
                await _repo.Update(newVilla);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = newVilla;
                return Ok(_response);
            } catch(Exception ex)
            {
                _logger.LogError("UpdateVilla", ex.Message);
                _response.IsSucceeded = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }
        }


        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdatePartialVilla(int id, JsonPatchDocument<UpdateVillaDTO> updateVillaDTO)
        {
            if (updateVillaDTO == null) return BadRequest();
            // AsNoTracking() se debe agregar para que EF, no le de seguimiento
            //var villaFounded = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
            var villaFounded = await _repo.Get(v => v.Id == id, tracked: false);

            if (villaFounded == null) return NotFound();

            UpdateVillaDTO newVilla = _mapper.Map<UpdateVillaDTO>(villaFounded);

            updateVillaDTO.ApplyTo(newVilla, ModelState);
            if (!ModelState.IsValid) return BadRequest();

            Villa villaModel = _mapper.Map<Villa>(newVilla);
            villaModel.Id = id;
            //_db.Villas.Update(villaModel);
            //await _db.SaveChangesAsync();
            await _repo.Update(villaModel);

            return Ok(villaModel.Id);
        }
    }
}
