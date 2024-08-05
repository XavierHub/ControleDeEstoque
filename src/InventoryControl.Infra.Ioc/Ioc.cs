using EmpXpo.Accounting.Repository;
using InventoryControl.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryControl.Infra.Ioc
{
    public static class Ioc
    {
        public static IServiceCollection AddIoc(
                this IServiceCollection services,
                IConfiguration configuration
            )
        {
            services.AddApplication();
            services.AddDapperRepository(o => o.ConnectionString = configuration["ConnectionStrings:cnDbInventory"] ?? "");

            return services;
        }
    }
}
