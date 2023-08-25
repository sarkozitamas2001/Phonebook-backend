using PhoneBookBackend.Data;
using PhoneBookBackend.Models;

public class SeedData
{
    public static void Initialize(DataContext context)
    {
        if (!context.People.Any())
        {

            var counties = new List<County>
            {
                new County { Name = "County 1" },
                new County { Name = "County 2" },
                new County { Name = "County 3" },
                new County { Name = "County 4" },
                new County { Name = "County 5" }
            };

            var cities = new List<City>
            {
                new City { Name = "City 1", County = counties[0] },
                new City { Name = "City 2", County = counties[1] },
                new City { Name = "City 3", County = counties[2] },
                new City { Name = "City 4", County = counties[3] },
                new City { Name = "City 5", County = counties[4] }
            };

            var people = new List<Person>
            {
                new Person
                {
                    Name = "Person 1",
                    PhoneNumber = "111111111",
                    City = cities[0]
                },
                new Person
                {
                    Name = "Person 2",
                    PhoneNumber = "211111111",
                    City = cities[1]
                },
                new Person
                {
                    Name = "Person 3",
                    PhoneNumber = "311111111",
                    City = cities[2]
                },
                new Person
                {
                    Name = "Person 4",
                    PhoneNumber = "411111111",
                    City = cities[3]
                },
                new Person
                {
                    Name = "Person 5",
                    PhoneNumber = "511111111",
                    City = cities[4]
                }
            };

            context.Counties.AddRange(counties);
            context.SaveChanges();
            context.Cities.AddRange(cities);
            context.SaveChanges();
            context.People.AddRange(people);
            context.SaveChanges();
        }
    }
}
