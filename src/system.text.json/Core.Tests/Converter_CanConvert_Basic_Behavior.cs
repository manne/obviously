using FluentAssertions;
using Xunit;

namespace Obviously.System.Text.Json.Tests
{
    public class Converter_CanConvert_Basic_Behavior
    {
        [Fact]
        public void GivenOneType_WhenItHasOneConstructorWithNoParameter_ThenTheCheck_ShouldBeNegative()
        {
            var cut = new ImmutableConverter();
            cut.CanConvert(typeof(TestingBasicTypes.TypeWithConstructorAndNoParameter)).Should().BeFalse("the original converter provides this functionality");
        }

        [Fact]
        public void GivenOneType_WhenItHasTwoConstructors_ThenTheCheck_ShouldBeNegative()
        {
            var cut = new ImmutableConverter();
            cut.CanConvert(typeof(TestingBasicTypes.TypeWithTwoConstructors)).Should().BeFalse("we do not want to support type with only one constructor");
        }
    }
}
