using System;
using System.Text.Json;
using FluentAssertions;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Obviously.System.Text.Json.Tests
{
    public class Converter_Read_Basic_Behavior
    {
        private static readonly JsonSerializerOptions Settings;

        static Converter_Read_Basic_Behavior()
        {
            Settings = new JsonSerializerOptions();
            Settings.Converters.Add(new ImmutableConverter());
        }

        [Fact]
        public void GivenOneValidType_And_OneValidJson_WhenDeserializing_ThenNoException_ShouldBeThrown()
        {
            const string json = @"{ ""Foo"": ""bar""}";
            Action deserializing = () => JsonSerializer.Deserialize<TestingTypeMatchingTypes.Basic>(json, Settings);
            deserializing.Should().NotThrow();
        }

        [Fact]
        public void GivenOneValidType_And_OneValidJson_WhenDeserializing_ThenTheObject_ShouldContainTheCorrectPropertyValues()
        {
            const string json = @"{ ""Foo"": ""bar""}";
            JsonSerializer.Deserialize<TestingTypeMatchingTypes.Basic>(json, Settings).Foo.Should().Be("bar");
        }

        [Fact]
        public void GivenOneValidType_WithJsonAttributeOnTheProperty_WhenDeserializing_ThenTheObject_ShouldContainTheCorrectPropertyValues()
        {
            const string json = @"{ ""lol"": ""bar""}";
            JsonSerializer.Deserialize<TestingTypeMatchingTypes.BasicWithJsonPropertyName>(json, Settings).Foo.Should().Be("bar");
        }

        [Fact]
        public void GivenOneValidType_AndCamelCaseForProperties_WhenDeserializing_ThenTheObject_ShouldContainTheCorrectPropertyValues()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new ImmutableConverter());
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            const string json = @"{ ""foo"": ""bar""}";
            JsonSerializer.Deserialize<TestingTypeMatchingTypes.Basic>(json, options).Foo.Should().Be("bar");
        }

        [Fact]
        public void GivenOneValidType_AndTheSettingIgnoringPropertyNameCasing_WhenDeserializing_ThenTheObject_ShouldContainTheCorrectPropertyValues()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new ImmutableConverter());
            options.PropertyNameCaseInsensitive = true;
            const string json = @"{ ""foO"": ""bar""}";
            JsonSerializer.Deserialize<TestingTypeMatchingTypes.Basic>(json, options).Foo.Should().Be("bar");
        }

        [Fact]
        public void GivenOneValidTypeWithInt32Property_WhenDeserializing_ThenTheObject_ShouldContainTheCorrectPropertyValues()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new ImmutableConverter());
            options.PropertyNameCaseInsensitive = true;
            const string json = @"{ ""Dummy"": 123}";
            JsonSerializer.Deserialize<TestingBasicTypes.TypeWithOneInt32Property>(json, options).Dummy.Should().Be(123);
        }
    }
}
