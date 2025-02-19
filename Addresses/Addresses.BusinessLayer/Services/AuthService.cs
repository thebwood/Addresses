using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Addresses.BusinessLayer.Services.Interfaces;
using Addresses.DatabaseLayer.Repositories.Interfaces;
using Addresses.Domain.Common;
using Addresses.Domain.Dtos;
using Addresses.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Addresses.BusinessLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;

        public AuthService(IConfiguration configuration, IAuthRepository authRepository)
        {
            _configuration = configuration;
            _authRepository = authRepository;
        }

        public async Task<Result<string>> Authenticate(string username, string password)
        {
            var user = await _authRepository.GetUserByUsernameAsync(username);
            if (user == null || !await _authRepository.ValidateUserCredentialsAsync(username, password))
            {
                return new Result<string>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Message = "Invalid username or password",
                    Errors = new List<Error> { new Error("Unauthorized", "Invalid username or password") }
                };
            }

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Username) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Result<string>
            {
                StatusCode = HttpStatusCode.OK,
                Value = tokenHandler.WriteToken(token),
                Message = "Authentication successful"
            };
        }

        public async Task<Result> RegisterUser(UserModel user)
        {
            // Hash the password before saving (implement your own hashing logic)
            user.PasswordHash = HashPassword(user.PasswordHash);
            await _authRepository.CreateUserAsync(user);
            return new Result
            {
                StatusCode = HttpStatusCode.Created,
                Message = "User registered successfully"
            };
        }

        public async Task<Result<UserDto>> GetUserById(Guid userId)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new Result<UserDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "User not found",
                    Errors = new List<Error> { new Error("NotFound", "User not found") }
                };
            }

            return new Result<UserDto>
            {
                StatusCode = HttpStatusCode.OK,
                Value = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
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

        private string HashPassword(string password)
        {
            // Implement your password hashing logic here
            // For example, using BCrypt:
            // return BCrypt.Net.BCrypt.HashPassword(password);
            return password; // Placeholder, replace with actual hash logic
        }

        public async Task AddTokenToBlacklist(string token, DateTime expirationDate)
        {
            await _authRepository.AddTokenToBlacklistAsync(token, expirationDate);
        }
    }
}
