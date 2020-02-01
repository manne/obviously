using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using TestApp.Controllers;
using Xunit;
using Xunit.Abstractions;

namespace TestApp
{
    public class IntegrationTests : IClassFixture<WebAppFixture>
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly HttpClient _client;

        public IntegrationTests(WebAppFixture fixture, ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;

            _client = fixture.CreateClient();
        }

        [Theory]
        [InlineData("foo", 1)]
        [InlineData("foo", 2)]
        [InlineData("foo", -1)]
        public async Task TrivialOption(string name, int? page)
        {
            var stringBuilder = new StringBuilder("api/trivial/1/").Append(name);
            if (page.HasValue)
            {
                stringBuilder = stringBuilder.Append("?pageNumber=").Append(page.Value);
            }

            var result = await _client.GetAsync<TrivialOptions>(stringBuilder.ToString());
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
            result.Should().BeEquivalentTo(new TrivialOptions(name, page ?? 0));
        }

        [Fact]
        public async Task MandatoryParametersWithMissingValues_ShouldNotBeBound()
        {
            using var result = await _client.GetAsync("api/trivial/1/foo");
            var content = await result.Content.ReadAsStringAsync();
            content.Should().BeEmpty("the mandatory page number is not set and therefore the binding failed");
        }

        [Theory]
        [InlineData("foo", 0, "sub")]
        [InlineData("foo", 2, "sub")]
        [InlineData("hello", -1, "sub")]
        public async Task TrivialOption2(string name, int? page, string subName)
        {
            var stringBuilder = new StringBuilder("api/trivial/2/")
                .Append(name)
                .Append("?subName=").Append(subName);
            if (page.HasValue)
            {
                stringBuilder = stringBuilder.Append("&pageNumber=").Append(page.Value);
            }

            var result = await _client.GetAsync<TrivialOptions2>(stringBuilder.ToString());
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
            result.Should().BeEquivalentTo(new TrivialOptions2(name, page ?? 0, subName));
        }

        [Theory]
        [InlineData("foo", 3)]
        [InlineData("foo", 2)]
        [InlineData("foo", -1)]
        public async Task TrivialOption3(string name, int page)
        {
            var stringBuilder = new StringBuilder("api/trivial/3/")
                .Append(name)
                .Append("?no=").Append(page);

            var result = await _client.GetAsync<TrivialOptions3>(stringBuilder.ToString());
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
            result.Should().BeEquivalentTo(new TrivialOptions3(name, page));
        }

        [Fact]
        public async Task InvalidModelWithTwoConstructors()
        {
            Func<Task> action = async () => await _client.GetAsync<TrivialOptions>("api/invalidmodelwithtwoconstructors/foo");

            using (new AssertionScope())
            {
                var exceptionAssertions = await action.Should().ThrowAsync<Exception>();
                exceptionAssertions
                    .And
                        .Message.Should()
                            .Contain("Model bound complex types must not be abstract or value types and must have a parameterless constructor.");
            }
        }
    }
}
