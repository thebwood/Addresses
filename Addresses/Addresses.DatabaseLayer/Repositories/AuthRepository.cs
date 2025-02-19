using Addresses.DatabaseLayer.Data;
using Addresses.DatabaseLayer.Repositories.Interfaces;
using Addresses.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addresses.DatabaseLayer.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AddressDbContext _context;

        public AuthRepository(AddressDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            var user = await GetUserByUsernameAsync(username);
            if (user == null)
            {
                return false;
            }

            // Assuming you have a method to verify the password hash
            return VerifyPasswordHash(password, user.PasswordHash);
        }

        public async Task CreateUserAsync(UserModel user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(UserModel user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserModel> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task AssignRoleToUserAsync(Guid userId, Guid roleId)
        {
            var userRole = new UserRoleModel
            {
                UserId = userId,
                RoleId = roleId
            };

            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task AddTokenToBlacklistAsync(string token, DateTime expirationDate)
        {
            var blacklistedToken = new TokenBlacklistModel
            {
                Token = token,
                ExpirationDate = expirationDate
            };

            await _context.TokenBlacklist.AddAsync(blacklistedToken);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            return await _context.TokenBlacklist.AnyAsync(t => t.Token == token);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            // Implement your password hash verification logic here
            // For example, using BCrypt:
            // return BCrypt.Net.BCrypt.Verify(password, storedHash);
            return password == storedHash; // Placeholder, replace with actual hash verification
        }
    }
}
