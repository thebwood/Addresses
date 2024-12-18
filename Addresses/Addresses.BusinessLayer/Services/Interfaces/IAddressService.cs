using Addresses.Domain.Dtos;
using Addresses.Domain.Models;

namespace Addresses.BusinessLayer.Services.Interfaces
{
    public interface IAddressService
    {
        Task<AddressModel> GetAddressById(Guid id);
        Task<List<AddressModel>> GetAllAddresses();
        Task<List<AddressModel>> GetAddressesByFilter(GetAddressesRequestDTO requestDTO);
        Task<AddressModel> CreateAddress(AddressModel address);
        Task<AddressModel> UpdateAddress(AddressModel address);
        Task<bool> DeleteAddress(Guid id);
    }
}
