using AutoMapper;
using InventoryControl.Application.Commands.Products;
using InventoryControl.Domain;
using InventoryControl.WebApi.Models;

namespace InventoryControl.WebApi.Extensions
{
    public static class WebApiExtension
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            var cfg = new MapperConfigurationExpression();            
            cfg.CreateMap<ProductModel, CreateProductCommand>();
            cfg.CreateMap<ProductModel, UpdateProductCommand>();
            cfg.CreateMap<ProductModel, Product>().ReverseMap();

            var mapperConfig = new MapperConfiguration(cfg);
            services.AddSingleton<IMapper, Mapper>(x => new Mapper(mapperConfig));

            return services;
        }
    }
}
