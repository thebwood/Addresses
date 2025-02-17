using Addresses.DatabaseLayer.Repositories.Interfaces;
using Addresses.Domain.Dtos;
using Addresses.Domain.Models;
using Addresses.DatabaseLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace Addresses.DatabaseLayer.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AddressDbContext _addressDbContext;
        public AddressRepository(AddressDbContext addressDbContext)
        {
            _addressDbContext = addressDbContext;
        }

        public async Task<AddressModel> CreateAddress(AddressModel address)
        {
            _addressDbContext.Addresses.Add(address);
            await _addressDbContext.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAddress(Guid id)
        {
            AddressModel? address = await GetAddressById(id);
            if (address == null)
            {
                return false; // Address not found
            }

            _addressDbContext.Addresses.Remove(address);
            await _addressDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<AddressModel?> GetAddressById(Guid id)
        {
            AddressModel? address = await _addressDbContext.Addresses.SingleOrDefaultAsync(x => x.Id == id);
            return address;
        }

        public async Task<List<AddressModel>> GetAddressesByFilter(GetAddressesRequestDTO requestDTO)
        {
            var skip = requestDTO.PageNumber * requestDTO.PageSize;
            var take = requestDTO.PageSize;

            List<AddressModel> addresses = await _addressDbContext.Addresses
                .Where(a => a.StreetAddress.ToLower().Contains(requestDTO.SearchText.ToLower()) ||
                            a.City.ToLower().Contains(requestDTO.SearchText.ToLower()) ||
                            a.State.ToLower().Contains(requestDTO.SearchText.ToLower()))
                .OrderBy(a => a.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return addresses;
        }

        public async Task<List<AddressModel>> GetAllAddresses()
        {
            return await _addressDbContext.Addresses
                                                .Take(1000)
                                                .ToListAsync();
        }

        public async Task<AddressModel> UpdateAddress(AddressModel address)
        {
            _addressDbContext.Addresses.Update(address);
            await _addressDbContext.SaveChangesAsync();
            return address;
        }
    }
}
