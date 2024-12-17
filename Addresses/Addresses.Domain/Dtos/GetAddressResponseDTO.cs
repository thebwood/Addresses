namespace Addresses.Domain.DTOs
{
    public class GetAddressResponseDTO
    {
        public GetAddressResponseDTO()
        {
            Address = new AddressDTO();
        }

        public AddressDTO Address { get; set; }

    }
}
