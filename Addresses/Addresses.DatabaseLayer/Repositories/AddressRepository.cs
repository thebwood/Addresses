using Addresses.DatabaseLayer.Repositories.Interfaces;
using Addresses.Domain.Models;
using Addresses.Infrastructure.Data;
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
            _addressDbContext.Addresses.Remove(new AddressModel(id));
            await _addressDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<AddressModel?> GetAddressById(Guid id)
        {
            AddressModel? address = await _addressDbContext.Addresses.SingleOrDefaultAsync(x => x.Id == id);
            return address;
        }

        public async Task<List<AddressModel>> GetAllAddresses()
        {
            return await _addressDbContext.Addresses.ToListAsync();
        }

        public async Task<AddressModel> UpdateAddress(AddressModel address)
        {
            _addressDbContext.Addresses.Update(address);
            await _addressDbContext.SaveChangesAsync();
            return address;
        }
    }
}
