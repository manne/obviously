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
    }

    [SemanticType(typeof(Guid))]
    public partial class TrivialGuidSemanticType { }
}
