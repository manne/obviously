using Xunit;

namespace Obviously.SemanticTypes.StaticTests
{
#nullable enable
    [SemanticType(typeof(string), IsNullableType = true)]
    public partial class StringNullableSemanticType { }

    public class StringNullableSemanticTypeTests
    {
        [Fact]
        public void ShouldBeAccessible()
        {
            // ReSharper disable AssignmentIsFullyDiscarded
            _ = new StringNullableSemanticType(null);
            _ = new StringNullableSemanticType("foo");
        }
    }
}