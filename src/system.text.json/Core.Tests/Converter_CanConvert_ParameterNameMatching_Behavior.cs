using FluentAssertions;
using Xunit;

namespace Obviously.System.Text.Json.Tests
{
    public class Converter_CanConvert_ParameterNameMatching_Behavior
    {
        [Fact]
        public void GivenOneType_WhenItOneMatchingParameter_ThenTheCheck_ShouldBePositive()
        {
            var cut = new ImmutableConverter();
            var canConvert = cut.CanConvert(typeof(TestingNameMatchingTypes.WithOneMatchingParameter));
            canConvert.Should().BeTrue();
        }

        [Fact]
        public void GivenOneType_WhenItDoesNotHaveMatchingParameter_ThenTheCheck_ShouldBeNegative()
        {
            var cut = new ImmutableConverter();
            var canConvert = cut.CanConvert(typeof(TestingNameMatchingTypes.WithOneNonMatchingParameter));
            canConvert.Should().BeFalse();
        }
    }
}
