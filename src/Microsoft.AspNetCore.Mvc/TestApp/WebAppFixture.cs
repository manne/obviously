using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TestApp
{
    public class WebAppFixture : IDisposable
    {
        private readonly Lazy<Inner> _inner = new Lazy<Inner>();

        public HttpClient CreateClient() => _inner.Value.CreateClient();

        private class Inner : WebApplicationFactory<Startup>
        {
            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                builder.UseContentRoot(".");
            }

            protected override IWebHostBuilder CreateWebHostBuilder()
            {
                return new WebHostBuilder()
                    .UseDefaultServiceProvider((context, options) => options.ValidateScopes = true)
                    .UseStartup<Startup>();
            }
        }

        public void Dispose()
        {
            if (_inner.IsValueCreated)
            {
                _inner.Value.Dispose();
            }
        }
    }
}