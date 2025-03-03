using Addresses.DatabaseLayer.Data;
using Addresses.DatabaseLayer.Repositories.Interfaces;
using Addresses.Domain.Models;
using System.Data.Entity;

namespace Addresses.DatabaseLayer.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AddressDbContext _context;

        public AdminRepository(AddressDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserModel>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
