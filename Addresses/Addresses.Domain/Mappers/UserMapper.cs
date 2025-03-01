using Addresses.Domain.Dtos;
using Addresses.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addresses.Domain.Mappers
{
    public static class UserMapper
    {
        public static UserDTO MapUserModelToUserDTO(UserModel user)
        {
            return new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}
