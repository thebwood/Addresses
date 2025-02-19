namespace Addresses.Domain.Models
{
    public class TokenBlacklistModel
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
