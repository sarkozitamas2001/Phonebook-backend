using PhoneBookBackend.Models;

namespace PhoneBookBackend.Interfaces
{
    public interface ICityRepository
    {
        ICollection<City> GetCities();
        City GetCityByID(int Id);
        City GetCityByName(string name);
        bool CreateCity(City city);
        bool UpdateCity(City city);
        bool DeleteCity(City city);
        bool Save();
    }
}
