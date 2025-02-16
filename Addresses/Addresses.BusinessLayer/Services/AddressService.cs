using Addresses.BusinessLayer.Services.Interfaces;
using Addresses.DatabaseLayer.Repositories.Interfaces;
using Addresses.Domain.Common;
using Addresses.Domain.Dtos;
using Addresses.Domain.Models;
using System.Net;

namespace Addresses.BusinessLayer.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<Result<AddressModel>> CreateAddress(AddressModel address)
        {
            var createdAddress = await _addressRepository.CreateAddress(address);
            return new Result<AddressModel>(createdAddress, true, Error.None)
            {
                StatusCode = HttpStatusCode.Created,
                Message = "Address created successfully"
            };
        }

        public async Task<Result<bool>> DeleteAddress(Guid id)
        {
            var isDeleted = await _addressRepository.DeleteAddress(id);
            return new Result<bool>(isDeleted, true, Error.None)
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Address deleted successfully"
            };
        }

        public async Task<Result<AddressModel>> GetAddressById(Guid id)
        {
            var address = await _addressRepository.GetAddressById(id);
            if (address == null)
            {
                return new Result<AddressModel>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Address not found",
                    Errors = new List<Error> { new Error("Not Found", "Address not found") }
                };
            }
            return new Result<AddressModel>(address, true, Error.None)
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Address retrieved successfully"
            };
        }

        public async Task<Result<List<AddressModel>>> GetAddressesByFilter(GetAddressesRequestDTO requestDTO)
        {
            var addresses = await _addressRepository.GetAddressesByFilter(requestDTO);
            return new Result<List<AddressModel>>(addresses, true, Error.None)
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Addresses retrieved successfully"
            };
        }

        public async Task<Result<List<AddressModel>>> GetAllAddresses()
        {
            var addresses = await _addressRepository.GetAllAddresses();
            return new Result<List<AddressModel>>(addresses, true, Error.None)
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Addresses retrieved successfully"
            };
        }

        public async Task<Result<AddressModel>> UpdateAddress(AddressModel address)
        {
            var updatedAddress = await _addressRepository.UpdateAddress(address);
            return new Result<AddressModel>(updatedAddress, true, Error.None)
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Address updated successfully"
            };
        }
    }
}
