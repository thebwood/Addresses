using Addresses.BusinessLayer.Services.Interfaces;
using Addresses.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Addresses.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel loginModel)
        {
            var result = await _authService.Authenticate(loginModel.Username, loginModel.Password);

            if (!result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserModel userModel)
        {
            var result = await _authService.RegisterUser(userModel);
            return StatusCode((int)result.StatusCode, result);
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
            return Ok(new { message = "Logout successful" });
        }
    }
}

