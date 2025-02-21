using Microsoft.AspNetCore.Identity;

namespace Addresses.Domain.Models
{
    public class UserTokenModel : IdentityUserToken<Guid>
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

}
