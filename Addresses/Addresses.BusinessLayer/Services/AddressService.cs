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
            return new Result<AddressModel>()
            {
                Value = createdAddress,
                StatusCode = HttpStatusCode.Created,
                Message = "Address created successfully",
                Errors = new()
            };
        }

        public async Task<Result<bool>> DeleteAddress(Guid id)
        {
            var isDeleted = await _addressRepository.DeleteAddress(id);
            return new Result<bool>()
            {
                Value = isDeleted,
                StatusCode = HttpStatusCode.OK,
                Message = "Address deleted successfully",
                Errors = new()
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
            return new Result<AddressModel>()
            {
                Value = address,
                StatusCode = HttpStatusCode.OK,
                Message = "Address retrieved successfully",
                Errors = new()
            };
        }

        public async Task<Result<List<AddressModel>>> GetAddressesByFilter(GetAddressesRequestDTO requestDTO)
        {
            var addresses = await _addressRepository.GetAddressesByFilter(requestDTO);
            return new Result<List<AddressModel>>()
            {
                Value = addresses,
                StatusCode = HttpStatusCode.OK,
                Message = "Addresses retrieved successfully",
                Errors = new()
            };
        }

        public async Task<Result<List<AddressModel>>> GetAllAddresses()
        {
            var addresses = await _addressRepository.GetAllAddresses();
            return new Result<List<AddressModel>>()
            {
                Value = addresses,
                StatusCode = HttpStatusCode.OK,
                Message = "Addresses retrieved successfully",
                Errors = new()
            };
        }

        public async Task<Result<AddressModel>> UpdateAddress(AddressModel address)
        {
            var updatedAddress = await _addressRepository.UpdateAddress(address);
            return new Result<AddressModel>()
            {
                Value = updatedAddress,
                StatusCode = HttpStatusCode.OK,
                Message = "Address updated successfully",
                Errors = new()
            };
        }
    }
}
