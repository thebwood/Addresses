namespace Addresses.Domain.Models
{
    public class RefreshTokenModel
    {
        public UserModel User { get; set; }
        public string RefreshToken { get; set; }
    }
}
