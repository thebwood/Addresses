using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addresses.Domain.Dtos
{
    public class RefreshUserTokenRequestDTO
    {
        public UserDTO User { get; set; }
        public string RefreshToken { get; set; }
    }
}
