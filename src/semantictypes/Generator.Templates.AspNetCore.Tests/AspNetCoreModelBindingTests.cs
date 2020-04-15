using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
namespace Obviously.SemanticTypes.Generator.Templates.AspNetCore.Tests
{
    public sealed class AspNetCoreModelBindingTests : IDisposable
    {
        private readonly WebApplicationFactory<Startup> _webApplicationFactory;
        private readonly HttpClient _httpClient;

        public AspNetCoreModelBindingTests()
        {
            _webApplicationFactory = new WebApplicationFactory<Startup>();
            _httpClient = _webApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task GivenOneStandardApp_WhenInvokingTheAutomaticGuidRoute_ThenTheResult_ShouldContain_TheGuidAsOneJsonString()
        {
            using var response = await _httpClient.GetAsync(new Uri("api/modelbinding/automatic/76447AA6-F77F-46AA-A4C6-0E4ABBC5A660", UriKind.Relative));
            var content = await response.Content.ReadAsStringAsync();
#pragma warning disable CA1308 // Normalize strings to uppercase
            content.Should().Be("\"76447AA6-F77F-46AA-A4C6-0E4ABBC5A660\"".ToLowerInvariant());
#pragma warning restore CA1308 // Normalize strings to uppercase
        }

        [Fact]
        public async Task GivenOneStandardApp_WhenInvokingTheManualGuidRoute_ThenTheResult_ShouldContain_TheGuidAsOneJsonString()
        {
            using var response = await _httpClient.GetAsync(new Uri("api/modelbinding/manual/76447AA6-F77F-46AA-A4C6-0E4ABBC5A660", UriKind.Relative));
            var content = await response.Content.ReadAsStringAsync();
#pragma warning disable CA1308 // Normalize strings to uppercase
            content.Should().Be("\"76447AA6-F77F-46AA-A4C6-0E4ABBC5A660\"".ToLowerInvariant());
#pragma warning restore CA1308 // Normalize strings to uppercase
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _webApplicationFactory.Dispose();
        }
    }
}
