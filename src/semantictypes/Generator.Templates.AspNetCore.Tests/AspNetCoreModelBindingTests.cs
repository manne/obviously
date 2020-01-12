using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Obviously.SemanticTypes.Generator.Templates.AspNetCore.Tests
{
    public class AspNetCoreModelBindingTests
    {
        [Fact]
        public async Task GivenOneStandardApp_WhenInvokingTheAutomaticGuidRoute_ThenTheResult_ShouldContain_TheGuidAsOneJsonString()
        {
            var webApplicationFactory = new WebApplicationFactory<Startup>();
            var httpClient = webApplicationFactory.CreateClient();
            using var response = await httpClient.GetAsync(new Uri("api/modelbinding/automatic/76447AA6-F77F-46AA-A4C6-0E4ABBC5A660", UriKind.Relative));
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("\"76447AA6-F77F-46AA-A4C6-0E4ABBC5A660\"".ToLowerInvariant());
        }

        [Fact]
        public async Task GivenOneStandardApp_WhenInvokingTheManualGuidRoute_ThenTheResult_ShouldContain_TheGuidAsOneJsonString()
        {
            var webApplicationFactory = new WebApplicationFactory<Startup>();
            var httpClient = webApplicationFactory.CreateClient();
            using var response = await httpClient.GetAsync(new Uri("api/modelbinding/manual/76447AA6-F77F-46AA-A4C6-0E4ABBC5A660", UriKind.Relative));
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("\"76447AA6-F77F-46AA-A4C6-0E4ABBC5A660\"".ToLowerInvariant());
        }
    }
}
