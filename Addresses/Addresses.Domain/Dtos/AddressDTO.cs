namespace Addresses.Domain.DTOs
{
    public class AddressDTO
    {
        public AddressDTO() { }
        public AddressDTO(Guid? id, string streetAddress, string? streetAddress2, string city, string state, string postalCode)
        {
            Id = id;
            StreetAddress = streetAddress;
            StreetAddress2 = streetAddress2;
            City = city;
            State = state;
            PostalCode = postalCode;
        }

        public Guid? Id { get; set; }
        public string StreetAddress { get; set; }
        public string? StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

    }
}
