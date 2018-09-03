using AspNetCore.SimpleApi.Model;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AspNetCore.SimpleApi.IntegartionTests.Controllers
{
    public class TodoControllerTests : ContollerTestBase
    {
        public TodoControllerTests(SimpleApiFactory simpleApiFactory, ITestOutputHelper testOutputHelper)
            : base(simpleApiFactory, testOutputHelper, "Todo", Api.Versions.V1)
        {
        }

        [Fact]
        public async Task ShouldValidateOnPost()
        {
            Todo todo = new Todo
            {
                Name = "",
                Description = "",
            };

            var endPoint = BuildContollerEndPointUrl();
            var response = await _httpClient.PostAsync(endPoint, new ObjectContent<Todo>(todo, new JsonMediaTypeFormatter()));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
