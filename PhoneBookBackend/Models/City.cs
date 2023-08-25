using System.ComponentModel.DataAnnotations;

namespace PhoneBookBackend.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int CountyId { get; set; }

        public County County { get; set; }
    }
}
