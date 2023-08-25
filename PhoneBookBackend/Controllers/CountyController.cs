using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhoneBookBackend.Dto;
using PhoneBookBackend.Interfaces;
using PhoneBookBackend.Models;

namespace PhoneBookBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountyController : Controller
    {
        private readonly ICountyRepository _countyRepository;
        private readonly IMapper _mapper;

        public CountyController(ICountyRepository countyRepository, IMapper mapper)
        {
            _countyRepository = countyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<County>))]
        public IActionResult GetCounties()
        {
            var counties = _mapper.Map<List<CountyDto>>(_countyRepository.GetCounties());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(counties);
        }

        [HttpGet("{id}")]
        public IActionResult GetCounty(int id)
        {
            var county = _mapper.Map<CountyDto>(_countyRepository.GetCountyById(id));

            if (county == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(county);
        }

        [HttpGet("byname/{name}")]
        public IActionResult GetCountyByName(string name)
        {
            var county = _mapper.Map<CountyDto>(_countyRepository.GetCountyByName(name));

            if (county == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(county);
        }

        [HttpPost]
        public IActionResult CreateCounty([FromBody] CountyDto newCounty)
        {
            if (newCounty == null)
            {
                return BadRequest(ModelState);
            }

            var county = _mapper.Map<County>(newCounty);

            try
            {
                if (!_countyRepository.CreateCounty(county))
                {
                    return StatusCode(500, ModelState);
                }
            }
            catch (Exception ex)
            {
                return Conflict(ex.InnerException.Message);
            }

            return StatusCode(201, "County created successfully.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCounty(int id)
        {
            var county = _countyRepository.GetCountyById(id);

            if (county == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_countyRepository.DeleteCounty(county))
            {
                ModelState.AddModelError("", "Something went wrong while deleting County");
            }

            return NoContent();
        }

    }
}
