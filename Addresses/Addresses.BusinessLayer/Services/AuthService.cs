using Addresses.BusinessLayer.Services.Interfaces;
using Addresses.DatabaseLayer.Repositories.Interfaces;
using Addresses.Domain.Common;
using Addresses.Domain.Dtos;
using Addresses.Domain.Mappers;
using Addresses.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Addresses.BusinessLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IPasswordHasher<UserModel> _passwordHasher;

        public AuthService(IConfiguration configuration, IAuthRepository authRepository, ILogger<AuthService> logger, IPasswordHasher<UserModel> passwordHasher)
        {
            _configuration = configuration;
            _authRepository = authRepository;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<UserLoginResponseDTO>> Authenticate(UserLoginRequestDTO loginDto)
        {
            UserModel? user = await _authRepository.GetUserByUsernameAsync(loginDto.UserName);
            if (user == null || !CheckPassword(user, loginDto.Password))
            {
                _logger.LogWarning("Invalid username or password for user: {UserName}", loginDto.UserName);
                return CreateErrorResult<UserLoginResponseDTO>(HttpStatusCode.Unauthorized, "Invalid username or password");
            }
            UserDTO userDTO = UserMapper.MapUserModelToUserDTO(user);
            string? tokenString = await GenerateJwtToken(user.Id);
            string? refreshTokenString = await CreateRefreshToken(user.Id);


            return new Result<UserLoginResponseDTO>
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Login successful",
                Value = new UserLoginResponseDTO
                {
                    User = userDTO,
                    Token = tokenString,
                    RefreshToken = refreshTokenString
                }
            };
        }

        private bool CheckPassword(UserModel user, string password)
        {
            var result = (PasswordVerificationResult)(int)_passwordHasher.VerifyHashedPassword(user, user.PasswordHash ?? string.Empty, password);
            return result == PasswordVerificationResult.Success;
        }

        public async Task<Result> RegisterUser(UserRegisterDTO registerDTO)
        {
            UserModel user = new UserModel
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                PasswordHash = _passwordHasher.HashPassword(null, registerDTO.Password)
            };

            await _authRepository.CreateUserAsync(user);

            return new Result
            {
                StatusCode = HttpStatusCode.Created,
                Message = "User registered successfully"
            };
        }

        public async Task<Result<UserDTO>> GetUserById(Guid userId)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", userId);
                return CreateErrorResult<UserDTO>(HttpStatusCode.NotFound, "User not found");
            }

            return new Result<UserDTO>
            {
                StatusCode = HttpStatusCode.OK,
                Value = new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                },
                Message = "User retrieved successfully"
            };
        }

        public async Task<Result> AssignRoleToUser(Guid userId, Guid roleId)
        {
            await _authRepository.AssignRoleToUserAsync(userId, roleId);
            return new Result
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Role assigned to user successfully"
            };
        }

        public async Task AddTokenToBlacklist(string token, DateTime expirationDate)
        {
            await _authRepository.AddTokenToBlacklistAsync(token, expirationDate);
        }

        private async Task<string> CreateRefreshToken(Guid userId)
        {
            var newRefreshToken = GenerateRefreshToken();
            await _authRepository.StoreTokenAsync(userId, newRefreshToken, DateTime.UtcNow.AddDays(7));

            return newRefreshToken;
        }


        public async Task<Result<RefreshUserTokenResponseDTO>> RefreshToken(RefreshUserTokenRequestDTO requestDTO)
        {
            var user = await _authRepository.GetUserByIdAsync(requestDTO.User.Id);
            if (user == null)
            {
                return CreateErrorResult<RefreshUserTokenResponseDTO>(HttpStatusCode.NotFound, "User not found");
            }

            var existingRefreshToken = await _authRepository.GetTokenByUserIdAsync(user.Id);
            if (existingRefreshToken != requestDTO.RefreshToken)
            {
                return CreateErrorResult<RefreshUserTokenResponseDTO>(HttpStatusCode.Unauthorized, "Invalid refresh token");
            }

            UserDTO userDTO = UserMapper.MapUserModelToUserDTO(user);
            var newRefreshToken = GenerateRefreshToken();
            await _authRepository.StoreTokenAsync(user.Id, newRefreshToken, DateTime.UtcNow.AddDays(7));

            var newJwtToken = await GenerateJwtToken(user.Id);

            return new Result<RefreshUserTokenResponseDTO>
            {
                StatusCode = HttpStatusCode.OK,
                Value = new RefreshUserTokenResponseDTO
                {
                    User = userDTO,
                    Token = newJwtToken,
                    RefreshToken = newRefreshToken
                },
                Message = "Token refreshed successfully"
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private async Task<string> GenerateJwtToken(Guid userId)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var roles = await _authRepository.GetRolesAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, GetUsersName(user)),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, string.Join(",", roles))

                };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        private string GetUsersName(UserModel user)
        {
            string firstName = user.FirstName ??= string.Empty;
            string lastName = user.LastName ??= string.Empty;
            return $"{firstName} {lastName}";
        }

        private Result<T> CreateErrorResult<T>(HttpStatusCode statusCode, string message, List<Error>? errors = null)
        {
            return new Result<T>
            {
                StatusCode = statusCode,
                Message = message,
                Errors = errors ?? new List<Error> { new Error("Error", message) }
            };
        }

        private Result CreateErrorResult(HttpStatusCode statusCode, string message, List<Error>? errors = null)
        {
            return new Result
            {
                StatusCode = statusCode,
                Message = message,
                Errors = errors ?? new List<Error> { new Error("Error", message) }
            };
        }
    }
}
