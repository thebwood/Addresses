using System.ComponentModel.DataAnnotations;

namespace Addresses.Domain.Models
{
    public class TokenBlacklistModel
    {
        [Key]
        [MaxLength(450)]
        public string Token { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
