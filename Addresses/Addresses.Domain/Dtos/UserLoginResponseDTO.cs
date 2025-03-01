namespace Addresses.Domain.Dtos
{
    public class UserLoginResponseDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public UserDTO User { get; set; }
    }
}
