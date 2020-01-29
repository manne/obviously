using System.Text.Json;
using FluentAssertions;
using Obviously.SemanticTypes.Generator;
using Xunit;

namespace Obviously.SemanticTypes.StaticTests
{
    public class SystemTextJsonTests
    {
        [Fact]
        public void ShouldSerialize()
        {
            var nuGetPackage = new NuGetPackage
            {
                PackageId = new SystemTextJsonPackageIdentifier("bla")
            };
            var cut = new SystemTextJsonPackageIdentifier.SystemTextJsonNuGetPackageConverter();
            var options = new JsonSerializerOptions();
            options.Converters.Add(cut);
            JsonSerializer.Serialize(nuGetPackage, typeof(NuGetPackage), options).Should().Contain("\"bla\"");
        } 

        [Fact]
        public void ShouldDeSerialize()
        {
            const string json = @"{""PackageId"": ""bla""}";
            var cut = new SystemTextJsonPackageIdentifier.SystemTextJsonNuGetPackageConverter();
            var options = new JsonSerializerOptions();
            options.Converters.Add(cut);
            var actualResult = (string)JsonSerializer.Deserialize<NuGetPackage>(json, options).PackageId;
            actualResult.Should().Be("bla");
        } 

        [Fact]
        public void ActualConverter_ShouldSerialize()
        {
            var nuGetPackage = new NuGetPackage
            {
                PackageId = new SystemTextJsonPackageIdentifier("bla")
            };
            var cut = new SystemTextJsonPackageIdentifier.SystemTextJsonConverter();
            var options = new JsonSerializerOptions();
            options.Converters.Add(cut);
            JsonSerializer.Serialize(nuGetPackage, typeof(NuGetPackage), options).Should().Contain("\"bla\"");
        }

        [Fact]
        public void ActualConverter_ShouldDeSerialize()
        {
            const string json = @"{""PackageId"": ""bla""}";
            var cut = new SystemTextJsonPackageIdentifier.SystemTextJsonConverter();
            var options = new JsonSerializerOptions();
            options.Converters.Add(cut);
            var actualResult = (string)JsonSerializer.Deserialize<NuGetPackage>(json, options).PackageId;
            actualResult.Should().Be("bla");
        }

        private class NuGetPackage
        {
            public SystemTextJsonPackageIdentifier PackageId { get; set; }
        }
    }

    [SemanticType(typeof(string))]   
    public sealed partial class SystemTextJsonPackageIdentifier
    {
#pragma warning disable CA1034 // Nested types should not be visible
#pragma warning disable IDE0001 // Name can be simplified
#pragma warning disable IDE0002 // Name can be simplified
        // ReSharper disable RedundantNameQualifier
        public sealed class SystemTextJsonNuGetPackageConverter : global::System.Text.Json.Serialization.JsonConverter<SystemTextJsonPackageIdentifier>
        {
            public override SystemTextJsonPackageIdentifier Read(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)
            {
                var value = global::System.Text.Json.JsonSerializer.Deserialize<string>(ref reader, options);
                return new SystemTextJsonPackageIdentifier(value);
            }

            public override void Write(global::System.Text.Json.Utf8JsonWriter writer, SystemTextJsonPackageIdentifier value, global::System.Text.Json.JsonSerializerOptions options)
            {
                if (value is null) throw new global::System.ArgumentNullException(nameof(value));
                global::System.Text.Json.JsonSerializer.Serialize(writer, value._value, typeof(string), options);
            }
        }
    }
}
