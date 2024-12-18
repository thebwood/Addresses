using Addresses.Domain.Dtos;
using Addresses.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Addresses.DatabaseLayer.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        Task<List<AddressModel>> GetAllAddresses();
        Task<List<AddressModel>> GetAddressesByFilter(GetAddressesRequestDTO requestDTO);
        Task<AddressModel?> GetAddressById(Guid id);
        Task<AddressModel> CreateAddress(AddressModel address);
        Task<AddressModel> UpdateAddress(AddressModel address);
        Task<bool> DeleteAddress(Guid id);
    }
}
