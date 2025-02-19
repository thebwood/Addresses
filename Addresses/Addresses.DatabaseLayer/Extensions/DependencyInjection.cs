using Addresses.DatabaseLayer.Repositories;
using Addresses.DatabaseLayer.Repositories.Interfaces;
using Addresses.DatabaseLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Addresses.DatabaseLayer.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabaseLayer(this IServiceCollection services, string connectionString)
        {
            // Register repositories
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<IAuthRepository, AuthRepository>();

            // Register DbContext
            services.AddDbContext<AddressDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
