using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace AspNetCore.SimpleApi.IntegartionTests
{
    public class SimpleApiFactory : WebApplicationFactory<Startup>
    {
        public HttpMessageHandler CreateHandler()
        {
            return Server.CreateHandler();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment(EnvironmentNameExt.Tests);
            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                //configurationBuilder.AddUserSecrets<Startup>();
                configurationBuilder.AddEnvironmentVariables();
            });
        }
    }
}
