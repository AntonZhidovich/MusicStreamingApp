namespace Identity.BusinessLogic.Models
{
    public class GetTokensRequest
    {
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
    }
}
