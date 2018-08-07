using AspNetCore.SimpleApi.Configuration.Options;
using AspNetCore.SimpleApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace AspNetCore.SimpleApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerApiDocs(this IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Simple API", Version = Versions.V1 });
            });

            return services;
        }


        public static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlServerOptions = configuration.GetSection(nameof(SqlServerOptions)).Get<SqlServerOptions>();

            services.AddDbContext<SimpleApiDbContext>(options => options.UseSqlServer(sqlServerOptions.ConnectionString));

            return services;
        }
    }
}
