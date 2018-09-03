using AspNetCore.SimpleApi.Configuration.Options;
using AspNetCore.SimpleApi.Filters;
using AspNetCore.SimpleApi.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Reflection;

namespace AspNetCore.SimpleApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentedMvcWithVersioning(this IServiceCollection services)
        {
            services
                .AddMvcCore()
                // Adds the versioned API explorer, which also adds IApiVersionDescriptionProvider service.
                .AddVersionedApiExplorer(
                    options =>
                    {
                        // The specified format code will format the version as "'v'major[.minor][-status]"
                        options.GroupNameFormat = "'v'VVV";

                        // note: this option is only necessary when versioning by URL segment. the SubstitutionFormat
                        // can also be used to control the format of the API version in route templates
                        options.SubstituteApiVersionInUrl = true;
                    });

            services.AddApiVersioning(options => options.ReportApiVersions = true);

            services.AddSwaggerGen(
                options =>
                {
                    // resolve the IApiVersionDescriptionProvider service
                    // note: that we have to build a temporary service provider here because one has not been created yet
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                    // add a swagger document for each discovered API version
                    // note: you might choose to skip or document deprecated API versions differently
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                    }

                    options.DescribeAllEnumsAsStrings();
                    options.AddFluentValidationRules();

                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerOperationFilter>();

                    // integrate xml comments
                    options.IncludeXmlComments(XmlCommentsFilePath);
                });

            return services;
        }

        private static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        private static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            {
                Title = $"Simple API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "A simple API with versioning and services.",
                Contact = new Contact() { Name = "Richárd Gyikó", Email = "gyiko.richard@outlook.com" },
                TermsOfService = "Shareware",
                License = new License() { Name = "MIT", Url = "https://opensource.org/licenses/MIT" }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }


        public static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlServerOptions = configuration.GetSection(nameof(SqlServerOptions)).Get<SqlServerOptions>();

            services.AddDbContext<SimpleApiDbContext>(options => options.UseSqlServer(sqlServerOptions.ConnectionString));

            return services;
        }
    }
}
