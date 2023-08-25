using PhoneBookBackend.Models;

namespace PhoneBookBackend.Interfaces
{
    public interface IPersonRepository
    {
        ICollection<Person> GetPeople();
        Person GetById(int Id);
        ICollection<Person> GetByName(string name);
        Person GetPersonByPhoneNumber(string phoneNumber);
        bool CreatePerson(Person person);
        bool DeletePerson(Person person);
        bool UpdatePerson(Person person);
        bool Save();
    }
}
