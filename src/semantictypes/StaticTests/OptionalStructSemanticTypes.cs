using System;
using Obviously.SemanticTypes.Generator;
using Xunit;

namespace Obviously.SemanticTypes.StaticTests
{
    [SemanticType(typeof(Guid?))]
    public partial class OptionalGuidSemanticTypes
    {
    }

    public class OptionalStructSemanticTypesTests
    {
        [Fact]
        public void ShouldBeAccessible()
        {
            // ReSharper disable AssignmentIsFullyDiscarded
            _ = new OptionalGuidSemanticTypes(null);
            _ = new OptionalGuidSemanticTypes(Guid.Empty);
        }
    }
}
