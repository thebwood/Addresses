using Microsoft.AspNetCore.Identity;

namespace Addresses.Domain.Models
{
    public class RoleModel : IdentityRole<Guid>
    {
        public ICollection<UserRoleModel> UserRoles { get; set; } = new List<UserRoleModel>();
    }
}
