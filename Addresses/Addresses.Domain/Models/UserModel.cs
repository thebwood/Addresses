using Microsoft.AspNetCore.Identity;
namespace Addresses.Domain.Models
{
    public class UserModel : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<UserRoleModel> UserRoles { get; set; } = new List<UserRoleModel>();
    }
}
