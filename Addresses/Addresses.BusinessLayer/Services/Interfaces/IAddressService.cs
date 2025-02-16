using Addresses.Domain.Common;
using Addresses.Domain.Dtos;
using Addresses.Domain.Models;

namespace Addresses.BusinessLayer.Services.Interfaces
{
    public interface IAddressService
    {
        Task<Result<AddressModel>> GetAddressById(Guid id);
        Task<Result<List<AddressModel>>> GetAllAddresses();
        Task<Result<List<AddressModel>>> GetAddressesByFilter(GetAddressesRequestDTO requestDTO);
        Task<Result<AddressModel>> CreateAddress(AddressModel address);
        Task<Result<AddressModel>> UpdateAddress(AddressModel address);
        Task<Result<bool>> DeleteAddress(Guid id);
    }
}
