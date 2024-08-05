using EmpXpo.Accounting.Application.Services;
using InventoryControl.Domain.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryControl.Application
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("InventoryControl.Application");
            services.AddMediatR(op =>
            {
                op.RegisterServicesFromAssemblies(assembly);
            });
            services.AddScoped<INotifierService, NotifierService>();

            return services;
        }
    }
}
