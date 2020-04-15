using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generators
{
    internal static class CompilationHelper
    {
        public static bool IsRelevantForNullability(this SemanticTypeGenerator.Input input) => !input.IsValueType && input.IsNullableType;
        public static bool HasType(this Compilation compilation, string fullyQualifiedMetadataName) =>
             compilation.GetTypeByMetadataName(fullyQualifiedMetadataName) != null;

        public static TypeSyntax MakeNullableIfEnabled(this TypeSyntax typeSyntax, SemanticTypeGenerator.Input input)
        {
            return input.IsNullableEnabled ? NullableType(typeSyntax) : typeSyntax;
        }

        public static TypeSyntax MakeNullableIfEnabledButNotIfStruct(this TypeSyntax typeSyntax, SemanticTypeGenerator.Input input)
        {
            var result = input.IsValueType ? typeSyntax : (input.IsNullableType ? NullableType(typeSyntax) : typeSyntax);
            return result;
        }

        public static string CreateFullNameWithNullableIfEnable(this SemanticTypeGenerator.Input input)
        {
            string result;
            if (!input.IsValueType)
            {
                result = input.ActualTypeFullName;
                if (input.IsNullableType)
                {
                    result += "?";
                }
            }
            else
            {
                result = input.ActualTypeFullName;
            }

            return result;
        }
    }
}
