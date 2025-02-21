using Addresses.BusinessLayer.Services.Interfaces;
using Addresses.Domain.Common;
using Addresses.Domain.Dtos;
using Addresses.Domain.DTOs;
using Addresses.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Addresses.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Apply authorization to the entire controller
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressDomainService;
        public AddressesController(IAddressService addressDomainService)
        {
            _addressDomainService = addressDomainService;
        }

        [HttpGet]
        public async Task<ActionResult<Result<GetAddressesResponseDTO>>> GetAllAddresses()
        {
            Result<List<AddressModel>> addresses = await _addressDomainService.GetAllAddresses();
            List<AddressDTO> addressDTOs = addresses.Value.Select(a => new AddressDTO(a.Id, a.StreetAddress, a.StreetAddress2, a.City, a.State, a.PostalCode)).ToList();

            Result<GetAddressesResponseDTO> response = new Result<GetAddressesResponseDTO>
            {
                Value = new GetAddressesResponseDTO { AddressList = addressDTOs },
                StatusCode = HttpStatusCode.OK,
                Message = "Addresses retrieved successfully"
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("filter")]
        public async Task<ActionResult<Result<GetAddressesResponseDTO>>> GetAddressesByFilter([FromBody] GetAddressesRequestDTO requestDTO)
        {
            Result<List<AddressModel>> addresses = await _addressDomainService.GetAddressesByFilter(requestDTO);
            List<AddressDTO> addressDTOs = addresses.Value.Select(a => new AddressDTO(a.Id, a.StreetAddress, a.StreetAddress2, a.City, a.State, a.PostalCode)).ToList();

            Result<GetAddressesResponseDTO>? response = new Result<GetAddressesResponseDTO>
            {
                Value = new GetAddressesResponseDTO { AddressList = addressDTOs },
                StatusCode = HttpStatusCode.OK,
                Message = "Addresses retrieved successfully"
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Result<GetAddressResponseDTO>>> GetAddressById(Guid id)
        {
            Result<AddressModel> address = await _addressDomainService.GetAddressById(id);
            if (!address.Success)
            {
                var notFoundResponse = new Result<GetAddressResponseDTO>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Address not found",
                    Errors = address.Errors
                };
                return NotFound(notFoundResponse);
            }

            AddressDTO addressDTO = new AddressDTO(address.Value.Id, address.Value.StreetAddress, address.Value.StreetAddress2, address.Value.City, address.Value.State, address.Value.PostalCode);
            var response = new Result<GetAddressResponseDTO>
            {
                Value = new GetAddressResponseDTO { Address = addressDTO },
                StatusCode = HttpStatusCode.OK,
                Message = "Address retrieved successfully"
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Result<AddressDTO>>> CreateAddress([FromBody] AddAddressRequestDTO requestDTO)
        {
            AddressModel address = new(null, requestDTO.Address.StreetAddress, requestDTO.Address.StreetAddress2, requestDTO.Address.City, requestDTO.Address.State, requestDTO.Address.PostalCode);
            Result<AddressModel> createdAddress = await _addressDomainService.CreateAddress(address);

            AddressDTO createdAddressDTO = new AddressDTO(createdAddress.Value.Id, createdAddress.Value.StreetAddress, createdAddress.Value.StreetAddress2, createdAddress.Value.City, createdAddress.Value.State, createdAddress.Value.PostalCode);
            var response = new Result<AddressDTO>
            {
                Value = createdAddressDTO,
                StatusCode = HttpStatusCode.Created,
                Message = "Address created successfully"
            };

            return CreatedAtAction(nameof(GetAddressById), new { id = createdAddressDTO.Id }, response);
        }

        [HttpPut]
        public async Task<ActionResult<Result<AddressDTO>>> UpdateAddress([FromBody] UpdateAddressRequestDTO requestDTO)
        {
            Result<AddressModel> addressResult = await _addressDomainService.GetAddressById(requestDTO.Address.Id.Value);
            if (!addressResult.Success)
            {
                var notFoundResponse = new Result<AddressDTO>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Address not found",
                    Errors = addressResult.Errors
                };
                return NotFound(notFoundResponse);
            }

            AddressModel address = addressResult.Value;
            address.StreetAddress = requestDTO.Address.StreetAddress;
            address.StreetAddress2 = requestDTO.Address.StreetAddress2;
            address.City = requestDTO.Address.City;
            address.State = requestDTO.Address.State;
            address.PostalCode = requestDTO.Address.PostalCode;

            Result<AddressModel> updatedAddress = await _addressDomainService.UpdateAddress(address);
            AddressDTO updatedAddressDTO = new AddressDTO(updatedAddress.Value.Id, updatedAddress.Value.StreetAddress, updatedAddress.Value.StreetAddress2, updatedAddress.Value.City, updatedAddress.Value.State, updatedAddress.Value.PostalCode);

            var response = new Result<AddressDTO>
            {
                Value = updatedAddressDTO,
                StatusCode = HttpStatusCode.OK,
                Message = "Address updated successfully"
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Result>> DeleteAddress(Guid id)
        {
            Result<AddressModel> addressResult = await _addressDomainService.GetAddressById(id);
            if (!addressResult.Success)
            {
                var notFoundResponse = new Result
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Address not found",
                    Errors = addressResult.Errors
                };
                return NotFound(notFoundResponse);
            }

            Result<bool> deleteResult = await _addressDomainService.DeleteAddress(id);
            var response = new Result
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Address deleted successfully"
            };

            return Ok(response);
        }
    }
}
