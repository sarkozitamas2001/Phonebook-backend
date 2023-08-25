using System.ComponentModel.DataAnnotations;

namespace PhoneBookBackend.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }
    }
}
