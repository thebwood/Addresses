namespace Addresses.Domain.Models
{
    public class AddressModel : EntityModel
    {
        public AddressModel()
        {
        }
        public AddressModel(Guid addressId) : base(addressId)
        {

        }
        public AddressModel(Guid? addressId, string streetAddress, string? streetAddress2, string city, string state, string postalCode)
            : base(addressId)
        {
            StreetAddress = streetAddress;
            StreetAddress2 = streetAddress2;
            City = city;
            State = state;
            PostalCode = postalCode;
        }

        public string StreetAddress { get; set; }
        public string? StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

    }
}
