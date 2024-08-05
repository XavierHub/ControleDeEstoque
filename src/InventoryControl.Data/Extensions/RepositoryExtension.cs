using Dapper.Dommel;
using InventoryControl.Domain.Abstractions.Repositories;
using InventoryControl.Infra.Data;
using InventoryControl.Infra.Data.Abstractions;
using InventoryControl.Infra.Data.Conventions;
using InventoryControl.Infra.Data.Options;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Data;

namespace EmpXpo.Accounting.Repository
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddDapperRepository(
                this IServiceCollection services,
                Action<DapperOptions> options
            )
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), @"Please provide options.");

            services.Configure(options);

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDbConnection>(sp =>
            {
                var dapperOptions = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<DapperOptions>>().Value;
                return new MySqlConnection(dapperOptions.ConnectionString);
            });

            DommelMapper.SetTableNameResolver(new TableNameResolver());

            return services;
        }
    }
}
