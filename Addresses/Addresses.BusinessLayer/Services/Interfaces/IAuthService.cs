using Addresses.Domain.Common;
using Addresses.Domain.Dtos;
using Addresses.Domain.Models;

namespace Addresses.BusinessLayer.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<string>> Authenticate(string username, string password);
        Task<Result> RegisterUser(UserRegisterDTO user);
        Task<Result<UserDTO>> GetUserById(Guid userId);
        Task<Result> AssignRoleToUser(Guid userId, Guid roleId);
        Task AddTokenToBlacklist(string token, DateTime expirationDate);
    }
}
