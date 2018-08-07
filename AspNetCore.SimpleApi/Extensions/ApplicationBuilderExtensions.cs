using AspNetCore.SimpleApi.Infrastructure;
using AspNetCore.SimpleApi.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace AspNetCore.SimpleApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedDatabase(this IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SimpleApiDbContext>();

                if (dbContext.Todos.Any() == false)
                {
                    dbContext.Todos.AddRange(
                        Enumerable.Range(0, 10).Select(i => new Todo { Name = $"Todo {i}", Description = $"Description {i}" }));
                }
            }

            return applicationBuilder;
        }


        public static IApplicationBuilder UseSwaggerDocs(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple API V1");
            });

            return app;
        }
    }
}
