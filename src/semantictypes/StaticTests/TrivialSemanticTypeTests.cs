using System;
using FluentAssertions;
using Obviously.SemanticTypes.Attributes;
using Xunit;

namespace Obviously.SemanticTypes.StaticTests
{
    public class TrivialSemanticTypeTests
    {
        [Fact]
        public void Creation_ShouldNotThrow_AnyException()
        {
            var valueToAssign = Guid.NewGuid();
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
    }

    [SemanticType(typeof(Guid))]
    public partial class TrivialGuidSemanticType { }

    [SemanticType(typeof(int))]
    public partial class TrivialInt32SemanticType { }

    [SemanticType(typeof(int))]
    public sealed partial class TrivialSealedInt32SemanticType { }
}
