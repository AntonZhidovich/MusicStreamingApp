namespace Identity.BusinessLogic.Models
{
    public class UsersPageResponse
    {
        public int PagesCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<UserDto> users { get; set; } = null!; 
    }
}
