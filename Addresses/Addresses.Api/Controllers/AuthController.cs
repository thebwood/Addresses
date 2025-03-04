using Addresses.BusinessLayer.Services.Interfaces;
using Addresses.Domain.Common;
using Addresses.Domain.Dtos;
using Addresses.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace Addresses.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Apply authorization to the entire controller
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous] // Allow anonymous access to the login action
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDTO loginDto)
        {
            Result<UserLoginResponseDTO>? result = await _authService.Authenticate(loginDto);

            if (!result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }
            return Ok(result);
        }

        [HttpPost("register")]
        [AllowAnonymous] // Allow anonymous access to the register action
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO registerDto)
        {
            Result? result = await _authService.RegisterUser(registerDto);

            if (!result.Success)
            {
                var errors = result.Errors.Select(e => new Error(e.Code, e.Name)).ToList();
                return BadRequest(new Result
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "User registration failed",
                    Errors = errors
                });
            }

            return Ok(new Result
            {
                StatusCode = HttpStatusCode.OK,
                Message = "User registered successfully"
            });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var result = await _authService.GetUserById(userId);
            if (!result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            if (jwtToken != null)
            {
                var expirationDate = jwtToken.ValidTo;
                await _authService.AddTokenToBlacklist(token, expirationDate);
            }
            return Ok(new Result
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Logout successful"
            });
        }

        [HttpPost("refresh")]
        [AllowAnonymous] // Allow anonymous access to the refresh token action
        public async Task<IActionResult> RefreshToken([FromBody] RefreshUserTokenRequestDTO requestDTO)
        {
            Result<RefreshUserTokenResponseDTO>? result = await _authService.RefreshToken(requestDTO);

            if (!result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return Ok(result);
        }

    }
}

