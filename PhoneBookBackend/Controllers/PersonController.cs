using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhoneBookBackend.Dto;
using PhoneBookBackend.Interfaces;
using PhoneBookBackend.Models;

namespace PhoneBookBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : Controller
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ICountyRepository _countyRepository;
        private readonly IMapper _mapper;

        public PersonController(IPersonRepository personRepository, ICityRepository cityRepository, ICountyRepository countyRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _cityRepository = cityRepository;
            _countyRepository = countyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetPeople()
        {
            var people = _personRepository.GetPeople();
            var peopleMap = _mapper.Map<List<PersonDto>>(people);
            var peopleList = people.ToList();

            int length = peopleMap.Count;

            for (int i = 0; i < length; i++)
            {
                var city = _cityRepository.GetCityByID(peopleList[i].CityId);
                var county = _countyRepository.GetCountyById(city.CountyId);
                peopleMap[i].City = new CityDto
                {
                    Name = city.Name,
                    County = new CountyDto
                    {
                        Name = county.Name
                    }
                };
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(peopleMap);
        }

        [HttpGet("{id}")]
        public IActionResult GetPersonById(int id)
        {

            var person = _personRepository.GetById(id);
            var personMap = _mapper.Map<PersonDto>(person);
            
            if (personMap  == null)
            {
                return NotFound();
            }

            var city = _cityRepository.GetCityByID(person.CityId);
            var county = _countyRepository.GetCountyById(city.CountyId);

            personMap.City = new CityDto 
            { 
                Name = person.City.Name,
                County = new CountyDto 
                { 
                    Name = county.Name 
                } 
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(personMap);
        }

        [HttpGet("byname/{name}")]
        public IActionResult GetPeopleByName(string name)
        {
            var people = _personRepository.GetByName(name);
            var peopleMap = _mapper.Map<List<PersonDto>>(people);
            var peopleList = people.ToList();

            if (peopleMap.Count == 0)
            {
                return NotFound();
            }

            int length = peopleMap.Count;

            for (int i = 0; i < length; i++)
            {
                var city = _cityRepository.GetCityByID(peopleList[i].CityId);
                var county = _countyRepository.GetCountyById(city.CountyId);
                peopleMap[i].City = new CityDto
                {
                    Name = city.Name,
                    County = new CountyDto
                    {
                        Name = county.Name
                    }
                };
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(peopleMap);
        }

        [HttpGet("byphone/{phoneNumber}")]
        public IActionResult GetPersonByPhoneNumber(string phoneNumber)
        {
            var person = _personRepository.GetPersonByPhoneNumber(phoneNumber);
            var personMap = _mapper.Map<PersonDto>(person);

            if (personMap == null)
            {
                return NotFound();
            }

            var city = _cityRepository.GetCityByID(person.CityId);
            var county = _countyRepository.GetCountyById(city.CountyId);

            personMap.City = new CityDto
            {
                Name = person.City.Name,
                County = new CountyDto
                {
                    Name = county.Name
                }
            };


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(personMap);
        }

        [HttpPost]
        public IActionResult CreatePerson([FromBody] PersonDto newPerson)
        {
            if (newPerson == null)
            {
                return BadRequest(ModelState);
            }

            var people = _personRepository.GetPeople()
                .Where(p => p.PhoneNumber == newPerson.PhoneNumber).FirstOrDefault();

            if (people != null)
            {
                ModelState.AddModelError("", "Person already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var personMap = _mapper.Map<Person>(newPerson);

            personMap.City = _cityRepository.GetCityByName(newPerson.City.Name);

            if (personMap.City == null)
            {
                return StatusCode(404, "City does not exist!");
            }

            personMap.CityId = personMap.City.Id;

            try
            {
                if (!_personRepository.CreatePerson(personMap))
                {
                    return StatusCode(500, ModelState);
                }
            }
            catch (Exception ex)
            {
                return Conflict(ex.InnerException.Message);
            }

            return StatusCode(201, "Person created successfully.");
        }

        [HttpPut("{phoneNumber}")]
        public IActionResult UpdatePerson(string phoneNumber, [FromBody] PersonDto updatedPersonDto)
        {
            if (updatedPersonDto == null)
            {
                return BadRequest(ModelState);
            }

            var existingPerson = _personRepository.GetPersonByPhoneNumber(phoneNumber);
            if (existingPerson == null)
            {
                return NotFound();
            }

            var updatedPerson = _mapper.Map<Person>(updatedPersonDto);

            existingPerson.Name = updatedPerson.Name;
            existingPerson.PhoneNumber = updatedPerson.PhoneNumber;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = _cityRepository.GetCityByName(updatedPersonDto.City.Name);
            if (city == null)
            {
                return StatusCode(404, "City does not exist!");
            }

            existingPerson.CityId = city.Id;

            try
            {
                if (!_personRepository.UpdatePerson(existingPerson))
                {
                    return StatusCode(500, ModelState);
                }
            }
            catch (Exception ex)
            {
                return Conflict(ex.InnerException.Message);
            }

            return NoContent();
        }


        [HttpDelete]
        public IActionResult DeletePerson(int id)
        {
            var person = _personRepository.GetById(id);

            if (person == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_personRepository.DeletePerson(person))
            {
                ModelState.AddModelError("", "Something went wrong while deleting Person");
            }

            return NoContent();
        }
    }
}
