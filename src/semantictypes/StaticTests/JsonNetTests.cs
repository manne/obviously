using System;
using FluentAssertions;
using Newtonsoft.Json;
using Obviously.SemanticTypes.Generator;
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
            ((string)actualResult.PackageId).Should().Be("Newtonsoft.Json");
        }

        [Fact]
        public void ShouldSerialize()
        {
            var package = new NuGetPackage {PackageId = new PackageIdentifier("foo")};
            var cut = new PackageIdentifier.PackageIdentifierSemanticTypeConverter();
            Func<string> serialization = () => JsonConvert.SerializeObject(package, cut);
            serialization.Should().NotThrow().Subject.Should().Contain("\"foo\"");
        }

        private class NuGetPackage
        {
            public PackageIdentifier PackageId { get; set; }
        }
    }

    [SemanticType(typeof(string))]
    public sealed partial class PackageIdentifier
    {
        public sealed class PackageIdentifierSemanticTypeConverter : global::Newtonsoft.Json.JsonConverter<PackageIdentifier>
        {
            public override void WriteJson(global::Newtonsoft.Json.JsonWriter writer, PackageIdentifier value, global::Newtonsoft.Json.JsonSerializer serializer)
            {
                serializer.Serialize(writer, value._value);
            }

            public override PackageIdentifier ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, PackageIdentifier existingValue,
                bool hasExistingValue, global::Newtonsoft.Json.JsonSerializer serializer)
            {
                var deserialize = serializer.Deserialize<string>(reader);
                return new PackageIdentifier(deserialize);
            }
        }
    }
}
