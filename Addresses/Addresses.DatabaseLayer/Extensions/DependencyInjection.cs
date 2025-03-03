using Addresses.DatabaseLayer.Data;
using Addresses.DatabaseLayer.Repositories;
using Addresses.DatabaseLayer.Repositories.Interfaces;
using Addresses.Domain.Models;
using Microsoft.AspNetCore.Identity;
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
            services.AddTransient<IAdminRepository, AdminRepository>();

            // Register DbContext
            services.AddDbContext<AddressDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // Register Identity services using AddIdentityCore
            services.AddIdentityCore<UserModel>(options =>
            {
                // Configure Identity options here if needed
            })
            .AddRoles<RoleModel>()
            .AddEntityFrameworkStores<AddressDbContext>()
            .AddDefaultTokenProviders();

            // Add SignInManager
            services.AddScoped<SignInManager<UserModel>>();

            return services;
        }
    }
}
