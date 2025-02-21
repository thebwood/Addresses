using Microsoft.AspNetCore.Identity;

namespace Addresses.Domain.Models
{
    public class UserRoleModel : IdentityUserRole<Guid>
    {
        public UserModel User { get; set; }

        public RoleModel Role { get; set; }
    }
}
