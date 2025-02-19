using Addresses.BusinessLayer.Services.Interfaces;
using Addresses.BusinessLayer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Addresses.BusinessLayer.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddTransient<IAddressService, AddressService>();
            return services;
        }
    }
}
