using AspNetCore.SimpleApi.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace AspNetCore.SimpleApi.IntegartionTests
{
    public abstract class IntegrationTestBase : IClassFixture<SimpleApiFactory>, IDisposable
    {
        protected readonly ITestOutputHelper _testOutputHelper;
        protected readonly SimpleApiFactory _simpleApiFactory;
        protected readonly HttpClient _httpClient;

        public IntegrationTestBase(SimpleApiFactory simpleApiFactory, ITestOutputHelper testOutputHelper)
        {
            _simpleApiFactory = simpleApiFactory ?? throw new ArgumentNullException(nameof(simpleApiFactory));
            _testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));

            // Force the Server to be created.
            _httpClient = _simpleApiFactory.CreateDefaultClient();

            EnsureDatabaseCreated();
        }


        #region helpers
        private void EnsureDatabaseCreated()
        {
            using (var scope = _simpleApiFactory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SimpleApiDbContext>();
                context.Database.EnsureCreated();
            }
        }

        private void EnsureDatabaseDeleted()
        {
            using (var scope = _simpleApiFactory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SimpleApiDbContext>();
                context.Database.EnsureDeleted();
            }
        }
        #endregion


        #region IDisposable implementation
        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    EnsureDatabaseDeleted();

                    _httpClient.Dispose();
                    _simpleApiFactory.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
