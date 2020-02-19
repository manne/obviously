using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generator
{
    internal static class CompilationHelper
    {
        public static bool HasType(this Compilation compilation, string fullyQualifiedMetadataName) =>
             compilation.GetTypeByMetadataName(fullyQualifiedMetadataName) != null;

        public static TypeSyntax MakeNullableIfEnabled(this TypeSyntax typeSyntax, bool isEnabled)
        {
            return isEnabled ? NullableType(typeSyntax) : typeSyntax;
        }
    }
}
