﻿using AutoMapper;
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
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.LogInformation("Getting all villas");
            IEnumerable<Villa> villaList = await _repo.GetAll();
            //IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<VillaDTO>>(villaList));
        }


        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0) return BadRequest("id 0 is not allow¡");

            var villaFounded = await _repo.Get(x => x.Id == id);
            //var villaFounded = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villaFounded == null) return NotFound("Villa not Found¡");
            return Ok(_mapper.Map<VillaDTO>(villaFounded));
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] CreateVillaDTO createDTO)
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

            //await _db.Villas.AddAsync(newVilla);
            //await _db.SaveChangesAsync();
            await _repo.Create(newVilla);
            return CreatedAtRoute("GetVilla", new { id = newVilla.Id }, newVilla);
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // utilizamos IActionResult porque no necesitamos devolver
        // un modelo. Retornaremos un NoContent
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0) return BadRequest(ModelState);

            //var villaFounded = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
            var villaFounded = await _repo.Get(v => v.Id == id);
            if (villaFounded == null) return NotFound();

            //_db.Villas.Remove(villaFounded);
            //await _db.SaveChangesAsync();
            _repo.Remove(villaFounded);
            return NoContent();
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> UpdateVilla(int id, [FromBody] UpdateVillaDTO updateVillaDTO)
        {
            if (updateVillaDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //var villaFounded = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
            var villaFounded = await _repo.Get(v => v.Id == id, tracked:false);
            if (villaFounded == null) return NotFound();

            Villa newVilla = _mapper.Map<Villa>(updateVillaDTO);
            newVilla.Id = id;
            //_db.Update(newVilla);
            //await _db.SaveChangesAsync();
            _repo.Update(newVilla);
            return Ok(newVilla);
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
            _repo.Update(villaModel);

            return Ok(villaModel.Id);
        }
    }
}
