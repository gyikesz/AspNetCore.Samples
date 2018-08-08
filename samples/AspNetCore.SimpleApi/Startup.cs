using AspNetCore.SimpleApi.Extensions;
using AspNetCore.SimpleApi.Model.Validation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.SimpleApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(validation =>
                {
                    validation.RegisterValidatorsFromAssemblyContaining<TodoValidator>();
                    validation.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });

            services.AddSqlServer(Configuration);

            services.AddSwaggerApiDocs();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwaggerDocs();

            app.UseMvc();

            if (env.IsDevelopment())
            {
                app.SeedDatabase();
            }
        }
    }
}
