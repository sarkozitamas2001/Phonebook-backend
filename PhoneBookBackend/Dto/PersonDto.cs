namespace PhoneBookBackend.Dto
{
    public class PersonDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public CityDto City { get; set; }    
    }
}
