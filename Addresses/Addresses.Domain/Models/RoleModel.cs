namespace Addresses.Domain.Models
{
    public class RoleModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserRoleModel> UserRoles { get; set; }
    }
}
