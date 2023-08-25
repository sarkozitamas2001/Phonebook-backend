using Microsoft.EntityFrameworkCore;
using PhoneBookBackend.Data;
using PhoneBookBackend.Interfaces;
using PhoneBookBackend.Models;

namespace PhoneBookBackend.Repository
{
    public class CountyRepository : ICountyRepository
    {
        private readonly DataContext _dataContext;

        public CountyRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool CreateCounty(County county)
        {
            _dataContext.Add(county);
            return Save();
        }

        public bool DeleteCounty(County county)
        {
            foreach (var city in _dataContext.Cities.Where(c => c.CountyId == county.Id).ToList())
            {
                foreach (var person in _dataContext.People.Where(p => p.CityId == city.Id).ToList())
                {
                    _dataContext.Remove(person);
                }
                _dataContext.Remove(city);
            }

            _dataContext.Remove(county);

            return Save();
        }

        public ICollection<County> GetCounties()
        {
            return _dataContext.Counties.OrderBy(c => c.Id).ToList();
        }

        public County GetCountyById(int id)
        {
            return _dataContext.Counties.Where(c => c.Id == id).FirstOrDefault();
        }

        public County GetCountyByName(string name)
        {
            return _dataContext.Counties.Where(c => c.Name == name).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();

            return saved > 0 ? true : false;
        }

        public bool UpdateCounty(County county)
        {
            _dataContext.Update(county);

            return Save();
        }
    }
}
