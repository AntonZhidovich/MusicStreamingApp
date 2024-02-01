using Microsoft.AspNetCore.Identity;

namespace Identity.DataAccess.Entities
{
    public class User : IdentityUser 
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public string Region { get; set; } = null!;
        public RefreshToken RefreshToken { get; set; }
    }
}
