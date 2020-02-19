using System;
using FluentAssertions;
using Obviously.SemanticTypes.Generator;
using Xunit;

namespace Obviously.SemanticTypes.StaticTests
{
    public class TrivialSemanticTypeTests
    {
        [Fact]
        public void Creation_ShouldNotThrow_AnyException()
        {
            var valueToAssign = Guid.NewGuid();
            // ReSharper disable once AssignmentIsFullyDiscarded
            Action creation = () => _ = new TrivialGuidSemanticType(valueToAssign);
            creation.Should().NotThrow();
        }

        [Fact]
        public void Comparison_GivenTwoEqualInt32_WhenComparing_TheSemanticComparison_ShouldBeZero()
        {
            var first = new TrivialInt32SemanticType(1);
            var second = new TrivialInt32SemanticType(1);
            first.Should().Be(second);
        }

        [Fact]
        public void GivenOneSealedClass_WhenAugmented_TheResultingClass_ShouldStillBeSealed()
        {
            var sealedType = typeof(TrivialSealedInt32SemanticType);
            sealedType.Should().BeSealed();
        }

        [Fact]
        public void GivenOneValidatingSemanticType_WhenTheValueIsInvalid_ThenTheConstructor_ShouldThrowOneArgumentException()
        {
            // ReSharper disable once AssignmentIsFullyDiscarded
            Action construction = () => _ = new StringValidatingSemanticType("invalid");
            construction.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GivenOneValidatingSemanticType_WhenTheValueIsValid_ThenTheConstructor_ShouldNotThrowAnyException()
        {
            // ReSharper disable once AssignmentIsFullyDiscarded
            Action construction = () => _ = new StringValidatingSemanticType("is valid");
            construction.Should().NotThrow();
        }

        [Fact]
        public void GivenOneInt32SemanticType_WhenInvokingToString_ThenTheToStringMethodOfTheActualValue_ShouldBeInvoked()
        {
            var cut = new TrivialInt32SemanticType(1337);
            cut.ToString().Should().Be("1337");
        }

        [Fact]
        public void GivenOneInt32SemanticType_WhenCheckingForBeingLower_ThenTheExpression_ShouldBeInvocable()
        {
            var first = new TrivialInt32SemanticType(1);
            var second = new TrivialInt32SemanticType(2);
            var comparison = first < second;
            comparison.Should().BeTrue();
        }
    }

    [SemanticType(typeof(Guid))]
    public partial class TrivialGuidSemanticType { }

    [SemanticType(typeof(int))]
    public sealed partial class TrivialSealedInt32SemanticType { }

    [SemanticType(typeof(int))]
    public partial class TrivialInt32SemanticType { }

#nullable enable
    [SemanticType(typeof(string))]
    public partial class StringNullableEnabledSemanticType { }
#nullable restore
}
