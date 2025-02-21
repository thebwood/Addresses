using Addresses.Domain.Models;

namespace Addresses.DatabaseLayer.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<UserModel> GetUserByUsernameAsync(string username);
        Task<bool> ValidateUserCredentialsAsync(string username, string password);
        Task CreateUserAsync(UserModel user);
        Task UpdateUserAsync(UserModel user);
        Task<UserModel> GetUserByIdAsync(Guid userId);
        Task AssignRoleToUserAsync(Guid userId, Guid roleId);
        Task AddTokenToBlacklistAsync(string token, DateTime expirationDate);
        Task<bool> IsTokenBlacklistedAsync(string token);
        Task StoreTokenAsync(Guid id, string tokenString, DateTime value);
        Task<string> GetTokenByUserIdAsync(Guid id);
    }
}
