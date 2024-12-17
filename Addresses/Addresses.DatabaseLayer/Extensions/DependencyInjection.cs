using Addresses.DatabaseLayer.Repositories;
using Addresses.DatabaseLayer.Repositories.Interfaces;
using Addresses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Addresses.DatabaseLayer.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabaseLayer(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IAddressRepository, AddressRepository>();

            services.AddDbContext<AddressDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            return services;

        }
    }
}
