using Addresses.Domain.Models;

namespace Addresses.DatabaseLayer.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<UserModel>> GetUsers();
    }
}
