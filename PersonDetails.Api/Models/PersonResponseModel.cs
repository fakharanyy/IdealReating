
namespace PersonDetails.Models
{
    public class PersonResponseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelephoneCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public IAsyncEnumerable<char>? Name { get; set; }
    }
}
