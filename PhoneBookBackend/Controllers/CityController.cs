using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhoneBookBackend.Dto;
using PhoneBookBackend.Interfaces;
using PhoneBookBackend.Models;

namespace PhoneBookBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : Controller
    {
        private readonly ICityRepository _cityRepository;
        private readonly ICountyRepository _countyRepository;
        private readonly IMapper _mapper;

        public CityController(ICityRepository cityRepository, ICountyRepository countyRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _countyRepository = countyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cities = _cityRepository.GetCities();
            var citiesMap = _mapper.Map<List<CityDto>>(cities);
            var citiesList = cities.ToList();

            if (citiesMap.Count == 0)
            {
                return NotFound();
            }
            
            int length = citiesList.Count;

            for (int i = 0; i < length; i++)
            {
                var county = _countyRepository.GetCountyById(citiesList[i].CountyId);

                citiesMap[i].County = new CountyDto
                {
                    Name = county.Name,
                };
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(citiesMap);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var city = _cityRepository.GetCityByID(id);
            var cityMap = _mapper.Map<CityDto>(city);

            if (cityMap == null)
            {
                return NotFound();
            }

            var county = _countyRepository.GetCountyById(city.CountyId);

            cityMap.County = new CountyDto
            {
                Name = county.Name
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(cityMap);
        }

        [HttpGet("byname/{name}")]
        public IActionResult GetCityByName(string name)
        {
            var city = _cityRepository.GetCityByName(name);
            var cityMap = _mapper.Map<CityDto>(city);

            if (cityMap == null)
            {
                return NotFound();
            }

            var county = _countyRepository.GetCountyById(city.CountyId);

            cityMap.County = new CountyDto
            {
                Name = county.Name
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(cityMap);
        }

        [HttpPost]
        public IActionResult CreateCity([FromBody] CityDto newCity)
        {
            if (newCity == null)
            {
                return BadRequest(ModelState);
            }

            var cities = _cityRepository.GetCities()
                .Where(c => c.Name == newCity.Name).FirstOrDefault();

            if (cities != null)
            {
                ModelState.AddModelError("", "City already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cityMap = _mapper.Map<City>(newCity);

            cityMap.County = _countyRepository.GetCountyByName(newCity.County.Name);
            cityMap.CountyId = cityMap.County.Id;

            try
            {
                if (!_cityRepository.CreateCity(cityMap))
                {
                    return StatusCode(500, ModelState);
                }
            }
            catch (Exception ex)
            {
                return Conflict(ex.InnerException.Message);
            }

            return StatusCode(201, "City created successfully.");
        }

        [HttpDelete]
        public IActionResult DeleteCity(int id)
        {
            var city = _cityRepository.GetCityByID(id);

            if (city == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cityRepository.DeleteCity(city))
            {
                ModelState.AddModelError("", "Something went wrong while deleting City");
            }

            return NoContent();
        }
    }
}
