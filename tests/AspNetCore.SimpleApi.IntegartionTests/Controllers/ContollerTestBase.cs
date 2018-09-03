using System;
using Xunit.Abstractions;

namespace AspNetCore.SimpleApi.IntegartionTests.Controllers
{
    public abstract class ContollerTestBase : IntegrationTestBase
    {
        private readonly string _contollerName;
        private readonly string _apiVersion;

        public ContollerTestBase(
            SimpleApiFactory simpleApiFactory,
            ITestOutputHelper testOutputHelper,
            string controller,
            string apiVersion) : base(simpleApiFactory, testOutputHelper)
        {
            _contollerName = controller ?? throw new ArgumentNullException(nameof(controller));
            _apiVersion = apiVersion ?? throw new ArgumentNullException(nameof(apiVersion));
        }

        protected virtual string BuildContollerEndPointUrl(string controllerAction = null)
        {
            return $"api/v{_apiVersion}/{_contollerName}/{controllerAction ?? "" }";
        }

        protected virtual string BuildContollerEndPointUrl(string controller, string controllerAction)
        {
            return $"api/v{_apiVersion}/{controller}/{controllerAction}";
        }
    }
}
