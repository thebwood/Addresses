﻿using Addresses.Domain.Common;
using Addresses.Domain.Dtos;

namespace Addresses.BusinessLayer.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserLoginResponseDTO>> Authenticate(UserLoginRequestDTO login);
        Task<Result> RegisterUser(UserRegisterDTO user);
        Task<Result<UserDTO>> GetUserById(Guid userId);
        Task<Result> AssignRoleToUser(Guid userId, Guid roleId);
        Task AddTokenToBlacklist(string token, DateTime expirationDate);
        Task<Result<RefreshUserTokenResponseDTO>> RefreshToken(RefreshUserTokenRequestDTO requestDTO);
    }
}
