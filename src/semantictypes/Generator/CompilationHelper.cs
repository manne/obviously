using Microsoft.CodeAnalysis;

namespace Obviously.SemanticTypes.Generator
{
    internal static class CompilationHelper
    {
        public static bool HasType(this Compilation compilation, string fullyQualifiedMetadataName) =>
             compilation.GetTypeByMetadataName(fullyQualifiedMetadataName) != null;
    }
}
