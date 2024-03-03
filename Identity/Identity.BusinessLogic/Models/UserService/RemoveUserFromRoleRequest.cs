namespace Identity.BusinessLogic.Models.UserService
{
    public class RemoveUserFromRoleRequest
    {
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}
