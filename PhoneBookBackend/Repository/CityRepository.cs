using PhoneBookBackend.Data;
using PhoneBookBackend.Interfaces;
using PhoneBookBackend.Models;

namespace PhoneBookBackend.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly DataContext _dataContext;

        public CityRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool CreateCity(City city)
        {
            _dataContext.Add(city);

            return Save();
        }

        public bool DeleteCity(City city)
        {
            foreach (var person in _dataContext.People.Where(p => p.CityId == city.Id).ToList())
            {
                _dataContext.Remove(person);
            }

            _dataContext.Remove(city);

            return Save();
        }

        public ICollection<City> GetCities()
        {
            return _dataContext.Cities.OrderBy(c => c.Id).ToList();
        }

        public City GetCityByID(int Id)
        {
            return _dataContext.Cities.Where(c => c.Id == Id).FirstOrDefault();
        }

        public City GetCityByName(string name)
        {
            return _dataContext.Cities.Where(c => c.Name == name).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();

            return saved > 0 ? true : false;
        }

        public bool UpdateCity(City city)
        {
            _dataContext.Update(city);

            return Save();
        }
    }
}
