using Addresses.BusinessLayer.Services.Interfaces;
using Addresses.Domain.Common;
using Addresses.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Addresses.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Apply authorization to the entire controller
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            Result<UsersResponseDTO> result = await _adminService.GetUsers();
            return Ok(result);
        }
    }
}
