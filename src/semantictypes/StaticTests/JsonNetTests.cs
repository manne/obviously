using System;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Obviously.SemanticTypes.StaticTests
{
    public class JsonNetTests
    {
        [Fact]
        public void ShouldDeserialize()
        {
            const string json = @"{
                    ""PackageId"": ""Newtonsoft.Json"",
               ""Version"": ""10.0.4""
                }";
            var cut = new PackageIdentifier.PackageIdentifierSemanticTypeConverter();
            var actualResult = JsonConvert.DeserializeObject<NuGetPackage>(json, cut);
            ((string)actualResult!.PackageId).Should().Be("Newtonsoft.Json");
        }

        [Fact]
        public void ShouldSerialize()
        {
            var package = new NuGetPackage {PackageId = new PackageIdentifier("foo")};
            var cut = new PackageIdentifier.PackageIdentifierSemanticTypeConverter();
            Func<string> serialization = () => JsonConvert.SerializeObject(package, cut);
            serialization.Should().NotThrow().Subject.Should().Contain("\"foo\"");
        }

        [Fact]
        public void ActualConverter_ShouldDeserialize()
        {
            const string json = @"{
                    ""PackageId"": ""Newtonsoft.Json"",
               ""Version"": ""10.0.4""
                }";
            var cut = new PackageIdentifier.JsonNetConverter();
            var actualResult = JsonConvert.DeserializeObject<NuGetPackage>(json, cut);
            ((string)actualResult!.PackageId).Should().Be("Newtonsoft.Json");
        }

        [Fact]
        public void ActualConverter_ShouldSerialize()
        {
            var package = new NuGetPackage { PackageId = new PackageIdentifier("foo") };
            var cut = new PackageIdentifier.JsonNetConverter();
            Func<string> serialization = () => JsonConvert.SerializeObject(package, cut);
            serialization.Should().NotThrow().Subject.Should().Contain("\"foo\"");
        }

        private class NuGetPackage
        {
            public PackageIdentifier? PackageId { get; set; }
        }
    }

    [SemanticType(typeof(string))]
    public sealed partial class PackageIdentifier
    {
#pragma warning disable CA1034 // Nested types should not be visible
#pragma warning disable IDE0001 // Name can be simplified
        // ReSharper disable RedundantNameQualifier
        public sealed class PackageIdentifierSemanticTypeConverter : global::Newtonsoft.Json.JsonConverter<PackageIdentifier>
        {
            public override void WriteJson(global::Newtonsoft.Json.JsonWriter writer, PackageIdentifier value, global::Newtonsoft.Json.JsonSerializer serializer)
            {
                if (writer is null) throw new global::System.ArgumentNullException(nameof(writer));
                if (value is null) throw new global::System.ArgumentNullException(nameof(value));
                if (serializer is null) throw new global::System.ArgumentNullException(nameof(serializer));
                serializer.Serialize(writer, value._value);
            }

            public override PackageIdentifier ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, PackageIdentifier existingValue,
                bool hasExistingValue, global::Newtonsoft.Json.JsonSerializer serializer)
            {
                if (serializer is null) throw new global::System.ArgumentNullException(nameof(serializer));
                var deserialize = serializer.Deserialize<string>(reader);
                return new PackageIdentifier(deserialize);
            }
        }
    }
}
