using InventoryControl.Infra.Ioc;
using InventoryControl.WebApi.Extensions;
using InventoryControl.WebApi.Filters;
using InventoryControl.WebApi.Middlewares;
using Microsoft.OpenApi.Models;
using Serilog;

namespace ControleDeEstoque.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuração do log de diagnóstico do Serilog
            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

            // Add Serilog configuration
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.UseSerilog(); // Add this line to use Serilog as the logging provider

            builder.Configuration
                   .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables()
                   .AddCommandLine(args);

            // Add services to the container.
            builder.Services.AddWebApi()
                            .AddIoc(builder.Configuration)
                            .AddControllers(options =>
                                options.Filters.Add<NotificationFilter>()
                            )
                            .AddJsonOptions(options =>
                                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                            );

            builder.Services.AddEndpointsApiExplorer()
                            .AddSwaggerGen(options =>
                            {
                                options.OperationFilter<OptionalRouteParameterFilter>();
                                options.SwaggerDoc("v1", new OpenApiInfo
                                {
                                    Title = "Swagger Documentação Web API",
                                    Description = "API for generating the daily consolidated report",
                                    Contact = new OpenApiContact() { Name = "Jonh Doe", Email = "jonhdoe@email.com.br" },
                                    License = new OpenApiLicense()
                                    {
                                        Name = "MIT License",
                                        Url = new Uri("https://opensource.org/licenses/MIT")
                                    }
                                });
                                options.EnableAnnotations();
                            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>()
               .UseHttpsRedirection()
               .UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
