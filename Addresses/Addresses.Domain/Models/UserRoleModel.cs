namespace Addresses.Domain.Models
{
    public class UserRoleModel
    {
        public Guid UserId { get; set; }
        public UserModel User { get; set; }
        public Guid RoleId { get; set; }
        public RoleModel Role { get; set; }
    }
}
