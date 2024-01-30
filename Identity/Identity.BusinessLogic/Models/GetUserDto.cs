using System.Text.Json.Serialization;

namespace Identity.BusinessLogic.Models
{
    public class GetUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
