using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Addresses.BusinessLayer.Services.Interfaces;
using Addresses.DatabaseLayer.Repositories.Interfaces;
using Addresses.Domain.Common;
using Addresses.Domain.Dtos;
using Addresses.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Addresses.BusinessLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<UserModel> _userManager;
        private readonly IAuthRepository _authRepository;

        public AuthService(IConfiguration configuration, UserManager<UserModel> userManager, IAuthRepository authRepository)
        {
            _configuration = configuration;
            _userManager = userManager;
            _authRepository = authRepository;
        }

        public async Task<Result<string>> Authenticate(UserLoginRequestDTO loginDto)
        {
            UserModel? user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return new Result<string>
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Message = "Invalid username or password",
                    Errors = new List<Error> { new Error("Unauthorized", "Invalid username or password") }
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Optionally, store the token in the database
            await _authRepository.StoreTokenAsync(user.Id, tokenString, tokenDescriptor.Expires.Value);

            return new Result<string>
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Login successful",
                Value = tokenString
            };
        }

        public async Task<Result> RegisterUser(UserRegisterDTO registerDTO)
        {
            var user = new UserModel
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
                return new Result
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "User registration failed",
                    Errors = errors
                };
            }

            return new Result
            {
                StatusCode = HttpStatusCode.Created,
                Message = "User registered successfully"
            };
        }

        public async Task<Result<UserDTO>> GetUserById(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Result<UserDTO>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "User not found",
                    Errors = new List<Error> { new Error("NotFound", "User not found") }
                };
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
    }
}
