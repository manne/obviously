using Obviously.SemanticTypes.Generator;

namespace Obviously.SemanticTypes.StaticTests
{
    // this class also tests for needed namespaces in the generated file
    [SemanticType(typeof(string))]
    public sealed partial class StringValidatingSemanticType
    {
        public static bool IsValid(string value)
        {
            return value != "invalid";
        }
    }
}
