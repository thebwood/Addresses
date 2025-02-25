using Addresses.DatabaseLayer.Data;
using Addresses.DatabaseLayer.Repositories.Interfaces;
using Addresses.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.UserName == username).ConfigureAwait(false);
        }

        public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            var user = await GetUserByUsernameAsync(username).ConfigureAwait(false);
            if (user == null)
            {
                return false;
            }

            return VerifyPasswordHash(password, user.PasswordHash);
        }

        public async Task CreateUserAsync(UserModel user)
        {
            await _context.Users.AddAsync(user).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateUserAsync(UserModel user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<UserModel> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
        }

        public async Task AssignRoleToUserAsync(Guid userId, Guid roleId)
        {
            UserRoleModel userRole = new UserRoleModel
            {
                UserId = userId,
                RoleId = roleId
            };

            await _context.UserRoles.AddAsync(userRole).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task AddTokenToBlacklistAsync(string token, DateTime expirationDate)
        {
            var blacklistedToken = new TokenBlacklistModel
            {
                Token = token,
                ExpirationDate = expirationDate
            };

            await _context.TokenBlacklist.AddAsync(blacklistedToken).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            return await _context.TokenBlacklist.AsNoTracking().AnyAsync(t => t.Token == token).ConfigureAwait(false);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            // Implement your password hash verification logic here
            // For example, using BCrypt:
            // return BCrypt.Net.BCrypt.Verify(password, storedHash);
            return password == storedHash; // Placeholder, replace with actual hash verification
        }

        public async Task StoreTokenAsync(Guid userId, string tokenString, DateTime expirationDate)
        {
            var existingToken = await _context.UserTokens
                .SingleOrDefaultAsync(ut => ut.UserId == userId && ut.LoginProvider == "Bearer" && ut.Name == "JWT")
                .ConfigureAwait(false);

            if (existingToken != null)
            {
                existingToken.Token = tokenString;
                existingToken.ExpirationDate = expirationDate;
                _context.UserTokens.Update(existingToken);
            }
            else
            {
                var userToken = new UserTokenModel
                {
                    UserId = userId,
                    LoginProvider = "Bearer",
                    Name = "RefreshToken",
                    Token = tokenString,
                    ExpirationDate = expirationDate
                };

                await _context.UserTokens.AddAsync(userToken).ConfigureAwait(false);
            }

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<string> GetTokenByUserIdAsync(Guid userId)
        {
            var userToken = await _context.UserTokens
                .AsNoTracking()
                .Where(ut => ut.UserId == userId && ut.ExpirationDate > DateTime.UtcNow)
                .OrderByDescending(ut => ut.ExpirationDate)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return userToken?.Token;
        }

        public async Task<IEnumerable<ClaimsIdentity?>> GetRolesAsync(UserModel user)
        {
            List<UserRoleModel>? userRoles = await _context.UserRoles
                .AsNoTracking()
                .Where(ur => ur.UserId == user.Id)
                .Include(ur => ur.Role)
                .ToListAsync()
                .ConfigureAwait(false);

            var claimsIdentities = userRoles.Select(ur => new ClaimsIdentity(new[]
            {
                    new Claim(ClaimTypes.Role, ur.Role.Name)
                }));

            return claimsIdentities;
        }
    }
}
