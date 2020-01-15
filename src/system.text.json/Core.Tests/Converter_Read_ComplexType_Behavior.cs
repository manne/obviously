using System.Text.Json;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Obviously.System.Text.Json.Tests
{
    public class Converter_Read_ComplexType_Behavior
    {
        private static readonly JsonSerializerOptions Settings;

        static Converter_Read_ComplexType_Behavior()
        {
            Settings = new JsonSerializerOptions();
            Settings.Converters.Add(new ImmutableConverter());
        }

        [Theory]
        [InlineData(@"{ ""Foo"": ""bar"", ""Xyz"": ""bla""}")]
        [InlineData(@"{ ""Xyz"": ""bla"", ""Foo"": ""bar""}")]
        public void GivenOneValidTypeWithTwoProperties_WhenDeserializing_ThenTheObject_ShouldContainTheCorrectPropertyValues(string json)
        {
            var deserializeObject = JsonSerializer.Deserialize<TestingTypeComplexMatchingTypes.Basic>(json, Settings);
            using var _ = new AssertionScope();
            deserializeObject.Foo.Should().Be("bar");
            deserializeObject.Xyz.Should().Be("bla");
        }

        [Theory]
        [InlineData(@"{ ""Xyz"": ""bla"", ""Foo"": ""notbar"", ""Foo"": ""bar""}")]
        [InlineData(@"{ ""Foo"": ""notbar"", ""Xyz"": ""bla"",  ""Foo"": ""bar""}")]
        [InlineData(@"{ ""Foo"": ""notbar"", ""Foo"": ""bar"", ""Xyz"": ""bla""}")]
        public void GivenOneValidTypeWithTwoProperties_AndOneJsonContainingOnePropertyTwice_WhenDeserializing_ThenTheCorrespondingPropertyOfTheObject_ShouldContainTheCorrectValue(string json)
        {
            var deserializeObject = JsonSerializer.Deserialize<TestingTypeComplexMatchingTypes.Basic>(json, Settings);
            using var _ = new AssertionScope();
            deserializeObject.Foo.Should().Be("bar");
            deserializeObject.Xyz.Should().Be("bla");
        }

        [Theory]
        [InlineData(@"{ ""Foo"": ""bar"", ""Xyz"": ""bla"", ""NotInteresting"": ""__""}")]
        [InlineData(@"{ ""NotInteresting"": ""__"", ""Foo"": ""bar"", ""Xyz"": ""bla""}")]
        public void GivenOneValidTypeWithTwoProperties_AndOneJsonWithAnAdditionalProperty_WhenDeserializing_ThenTheObject_ShouldContainTheCorrectPropertyValues(string json)
        {
            var deserializeObject = JsonSerializer.Deserialize<TestingTypeComplexMatchingTypes.Basic>(json, Settings);
            using var _ = new AssertionScope();
            deserializeObject.Foo.Should().Be("bar");
            deserializeObject.Xyz.Should().Be("bla");
        }

        [Theory]
        [InlineData(@"{ ""Bar"": [""bar"", ""bar2""], ""Cool"": ""Cooler"", ""NotInteresting"": ""__""}")]
        [InlineData(@"{ ""Cool"": ""Cooler"", ""NotInteresting"": ""__"", ""Foo"": ""bar"", ""Bar"": [""bar"", ""bar2""]}")]
        public void GivenOneValidTypeWithTwoProperties_AndOneJsonWithAnAdditionalProperty_WhenDeserializing_ThenTheObject_ShouldContainTheCorrectPropertyValues2(string json)
        {
            var deserializeObject = JsonSerializer.Deserialize<TestingTypeComplexMatchingTypes.WithTwoProperties>(json, Settings);
            using var _ = new AssertionScope();
            deserializeObject.Bar.Should().ContainInOrder("bar", "bar2");
            deserializeObject.Cool.Should().Be("Cooler");
        }

        [Fact]
        public void GivenOneValidTypeWithTwoPropertiesOneOfItIsOneGuid_AndOneJsonWithAnAdditionalProperty_WhenDeserializing_ThenTheObject_ShouldContainTheCorrectPropertyValues()
        {
            const string json = @"{ ""Bar"": ""5AFF9859-A08C-4B3D-BD8D-57D01D6B795B"", ""Cool"": ""Cooler""}";
            var deserializeObject = JsonSerializer.Deserialize<TestingTypeComplexMatchingTypes.WithTwoProperties2>(json, Settings);
            using var _ = new AssertionScope();
            deserializeObject.Bar.Should().Be("5AFF9859-A08C-4B3D-BD8D-57D01D6B795B");
            deserializeObject.Cool.Should().Be("Cooler");
        }

        [Fact]
        public void GivenOneValidTypeWithOneGenericProperty_WhenDeserializing_ThenTheObject_ShouldContainTheCorrectPropertyValues()
        {
            const string json = @"{ ""Cool"": ""Cooler""}";
            var deserializeObject = JsonSerializer.Deserialize<TestingTypeComplexMatchingTypes.WithOneGenericProperties<string>>(json, Settings);
            using var _ = new AssertionScope();
            deserializeObject.Cool.Should().Be("Cooler");
        }
    }
}
