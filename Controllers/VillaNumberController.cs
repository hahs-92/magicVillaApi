using AutoMapper;
using MagicVilla_API.DTOs;
using MagicVilla_API.Models;
using MagicVilla_API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Xml.Linq;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberController: ControllerBase
    {
        private readonly ILogger<VillaNumberController> _logger;
        //private readonly ApplicationDbContext _db;
        private readonly IVillaRepository _repoVilla;
        private readonly IVillaNumberRepository _repoVillaNumber;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaNumberController(
            ILogger<VillaNumberController> logger,
            //ApplicationDbContext db,
            IVillaRepository repoVilla,
            IVillaNumberRepository repoVillaNumber,
            IMapper mapper
        )
        {
            _logger = logger;
            //_db= db;
            _repoVilla = repoVilla;
            _repoVillaNumber= repoVillaNumber;
            _mapper = mapper;
            _response = new();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        public async Task<ActionResult<APIResponse>> GetVillasNumber()
        {
            try
            {
                _logger.LogInformation("Getting all villasNumber");
                IEnumerable<VillaNumber> villaNumberList = await _repoVillaNumber.GetAll();

                _response.Result = _mapper.Map<IEnumerable<VillaNumberDTO>>(villaNumberList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetVillasNumber", ex.Message);
                _response.IsSucceeded = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }
        }


        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villaNumberFounded = await _repoVillaNumber.Get(x => x.VillaNo == id);
                
                if (villaNumberFounded == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSucceeded = false;
                    return NotFound(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumberFounded);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetVillaNumber", ex.Message);
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
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] CreateVillaNumberDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                //custom validation
                if (await _repoVillaNumber.Get(v => v.VillaNo == createDTO.VillaNo) != null)
                {
                    _logger.LogError("Name villa had already created¡");
                    ModelState.AddModelError("VillaNo Existed", "No villa had already created¡");
                    return BadRequest(ModelState);
                }

                if(await _repoVilla.Get(v => v.Id == createDTO.VillaId) == null)
                {
                    _logger.LogError("Villa Id no Exist¡");
                    ModelState.AddModelError("VillaId Not Existed", "Villa Id Not Founded¡");
                    return BadRequest(ModelState);
                }

                VillaNumber newVillaNumber = _mapper.Map<VillaNumber>(createDTO);
                newVillaNumber.CreatedAt = DateTime.Now;
                newVillaNumber.UpdatedAt = DateTime.Now;

                //await _db.Villas.AddAsync(newVilla);
                //await _db.SaveChangesAsync();
                await _repoVillaNumber.Create(newVillaNumber);
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = newVillaNumber;
                return CreatedAtRoute("GetVillaNumber", new { id = newVillaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateVillaNumber", ex.Message);
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
        public async Task<IActionResult> DeleteVillaNumber(int id)
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
                var villaNumberFounded = await _repoVillaNumber.Get(v => v.VillaNo == id);
                if (villaNumberFounded == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSucceeded = false;
                    return NotFound(_response);
                }

                //_db.Villas.Remove(villaFounded);
                //await _db.SaveChangesAsync();
                await _repoVillaNumber.Remove(villaNumberFounded);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteVillaNumber", ex.Message);
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
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] UpdateVillaNumberDTO updateVillaNumberDTO)
        {
            try
            {
                if (updateVillaNumberDTO == null) return BadRequest();
                if (!ModelState.IsValid) return BadRequest(ModelState);

                //var villaFounded = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
                var villaNumberFounded = await _repoVillaNumber.Get(v => v.VillaNo == id, tracked: false);
                if (villaNumberFounded == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSucceeded = false;
                    return NotFound(_response);
                }

                if(await _repoVilla.Get(v => v.Id == updateVillaNumberDTO.VillaId)== null)
                {
                    _logger.LogError("villaNo not Exist¡");
                    ModelState.AddModelError("VillaNo Not Existed", "No villa Not Founded¡");
                    return BadRequest(ModelState);
                }

                VillaNumber newVillaNumber = _mapper.Map<VillaNumber>(updateVillaNumberDTO);
                newVillaNumber.VillaNo = id;
                newVillaNumber.UpdatedAt = DateTime.Now;
                //_db.Update(newVilla);
                //await _db.SaveChangesAsync();
                await _repoVillaNumber.Update(newVillaNumber);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = newVillaNumber;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateVillaNumber", ex.Message);
                _response.IsSucceeded = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorsMessage = new List<string>() { ex.ToString() };
                return _response;
            }
        }
    }
}
