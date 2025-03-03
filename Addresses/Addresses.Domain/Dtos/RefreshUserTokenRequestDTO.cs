namespace Addresses.Domain.Dtos
{
    public class RefreshUserTokenRequestDTO
    {
        public UserDTO User { get; set; }
        public string RefreshToken { get; set; }
    }
}
