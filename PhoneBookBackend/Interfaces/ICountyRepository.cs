using PhoneBookBackend.Models;

namespace PhoneBookBackend.Interfaces
{
    public interface ICountyRepository
    {
        ICollection<County> GetCounties();
        County GetCountyById(int id);
        County GetCountyByName(string name);
        bool CreateCounty(County county);
        bool UpdateCounty(County county);
        bool DeleteCounty(County county);
        bool Save();
    }
}
