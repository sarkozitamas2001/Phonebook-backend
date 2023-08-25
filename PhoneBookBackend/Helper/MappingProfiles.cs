using AutoMapper;
using PhoneBookBackend.Dto;
using PhoneBookBackend.Models;

namespace PhoneBookBackend.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Person, PersonDto>();
            CreateMap<City, CityDto>();
            CreateMap<County, CountyDto>();
            CreateMap<PersonDto, Person>();
            CreateMap<CityDto, City>();
            CreateMap<CountyDto, County>();
        }
    }
}
