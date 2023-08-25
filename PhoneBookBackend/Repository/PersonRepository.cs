using PhoneBookBackend.Data;
using PhoneBookBackend.Interfaces;
using PhoneBookBackend.Models;

namespace PhoneBookBackend.Repository
{
    public class PersonRepository : IPersonRepository
    {

        private readonly DataContext _dataContext;

        public PersonRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool CreatePerson(Person person)
        {
            _dataContext.Add(person);

            return Save();

        }

        public bool DeletePerson(Person person)
        {
            _dataContext.Remove(person);

            return Save();
        }

        public Person GetById(int Id)
        {

            return _dataContext.People.Where(p => p.Id == Id).FirstOrDefault();
        }

        public ICollection<Person> GetByName(string name)
        {
            return _dataContext.People.OrderBy(p => p.Id).Where(p => p.Name.Contains(name)).ToList();
        }

        public ICollection<Person> GetPeople()
        {
            return _dataContext.People.OrderBy(p => p.Id).ToList();
        }

        public Person GetPersonByPhoneNumber(string phoneNumber)
        {
            return _dataContext.People.Where(p => p.PhoneNumber == phoneNumber).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePerson(Person person)
        {
            _dataContext.Update(person);
            return Save();
        }
    }
}
