using AutoMapper;
using Entities.Helpers;
using Entities.Models;
using Interfaces;
using LoggerService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Repository;
using System.IO;

namespace WebApiCore.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<RepositoryContext>(options
                => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IDataShaper<Book>, DataShaper<Book>>();
            services.AddScoped<ISortHelper<Book>, SortHelper<Book>>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "V1" });

                // Configure Swagger to use the xml documentation file
                var xmlFile = Path.ChangeExtension(typeof(Startup).Assembly.Location, ".xml");
                c.IncludeXmlComments(xmlFile);
            });
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
        }
    }
}
