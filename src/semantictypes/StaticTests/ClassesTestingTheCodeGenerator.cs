using Obviously.SemanticTypes.Generator;

namespace Obviously.SemanticTypes.StaticTests
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
    [SemanticType(typeof(int))]
    internal partial class InternalTrivialSealedInt32SemanticType { }
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
}
