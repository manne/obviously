using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Xunit;

namespace Obviously.System.Text.Json.Tests.Examples
{
    public class SimpleDeserialization
    {
        public sealed class ProgramSettings
        {
            public ProgramSettings(Uri baseUrl, string adminName, string adminPassword)
            {
                BaseUrl = baseUrl;
                AdminName = adminName;
                AdminPassword = adminPassword;
            }

            public Uri BaseUrl { get; }

            public string AdminName { get; }

            public string AdminPassword { get; }
        }

        [Fact]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        [SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "For documentation")]
        public void DeserializeSettings()
        {
            const string settingsJson = @"{""BaseUrl"": ""https://example.com:8999"", ""AdminName"": ""admin"", ""AdminPassword"": ""topsecret""}";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new ImmutableConverter());

            var programSettings = JsonSerializer.Deserialize<ProgramSettings>(settingsJson, options);
            var baseUrl = programSettings.BaseUrl; // https://example.com:8999
            var adminName = programSettings.AdminName; // admin
            var adminPassword = programSettings.AdminPassword; // topsecret
        }
    }
}
