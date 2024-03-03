using System.Text.Json.Serialization;

namespace Identity.BusinessLogic.Models.UserService
{
    public class RegisterUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public string Region { get; set; }
    }
}
