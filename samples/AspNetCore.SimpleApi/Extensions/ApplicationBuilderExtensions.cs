using AspNetCore.SimpleApi.Infrastructure;
using AspNetCore.SimpleApi.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
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

                    dbContext.SaveChanges();
                }
            }

            return applicationBuilder;
        }


        public static IApplicationBuilder UseSwaggerDocs(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });

            return app;
        }
    }
}
