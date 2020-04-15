using FluentAssertions;
using Xunit;

namespace Obviously.SemanticTypes.StaticTests
{
    [SemanticType(typeof(int))]
    internal partial class PrivateSemanticTypeForComparison { }

    public class ComparisonTests
    {
        [Fact]
        public void GivenOneNonNullInstance_WhenComparedLessToOneNullInstance_ThenTheResult_ShouldBeFalse()
        {
            var left = new PrivateSemanticTypeForComparison(1);
            PrivateSemanticTypeForComparison? right = null;
            // ReSharper disable once ExpressionIsAlwaysNull, reason: for testing
            (left < right!).Should().BeFalse();
        }

        [Fact]
        public void GivenOneNullInstance_WhenComparedLessToOneNullInstance_ThenTheResult_ShouldBeFalse()
        {
            PrivateSemanticTypeForComparison? left = null;
            PrivateSemanticTypeForComparison? right = null;
            // ReSharper disable ExpressionIsAlwaysNull, reason: for testing
            (left! < right!).Should().BeFalse();
            // ReSharper restore ExpressionIsAlwaysNull
        }

        [Fact]
        public void GivenOneNullInstance_WhenComparedLessToOneNonNullInstance_ThenTheResult_ShouldBeTrue()
        {
            PrivateSemanticTypeForComparison? left = null;
            var right = new PrivateSemanticTypeForComparison(1);
            // ReSharper disable once ExpressionIsAlwaysNull, reason: for testing
            (left! < right).Should().BeTrue();
        }
    }
}
