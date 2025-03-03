using Addresses.BusinessLayer.Services.Interfaces;
using Addresses.DatabaseLayer.Repositories.Interfaces;
using Addresses.Domain.Common;
using Addresses.Domain.Dtos;
using Addresses.Domain.Mappers;
using System.Net;

namespace Addresses.BusinessLayer.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Result<UsersResponseDTO>> GetUsers()
        {
            List<UserDTO> userDTOs = new List<UserDTO>();
            var users = await _adminRepository.GetUsers();

            foreach (var user in users)
            {
                userDTOs.Add(UserMapper.MapUserModelToUserDTO(user));
            }

            var usersResponseDTO = new UsersResponseDTO
            {
                Users = userDTOs
            };

            return new Result<UsersResponseDTO>
            {
                Value = usersResponseDTO,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
