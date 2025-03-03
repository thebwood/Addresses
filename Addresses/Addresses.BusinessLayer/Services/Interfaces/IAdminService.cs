using Addresses.Domain.Common;
using Addresses.Domain.Dtos;
using Addresses.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addresses.BusinessLayer.Services.Interfaces
{
    public interface IAdminService
    {
        Task<Result<UsersResponseDTO>> GetUsers();
    }
}
