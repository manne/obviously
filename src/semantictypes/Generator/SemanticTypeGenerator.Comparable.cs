using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generator
{
    // ReSharper disable once UnusedMember.Global, reason: utilized by the roslyn code generator
    public partial class SemanticTypeGenerator
    {
        private static (SimpleBaseTypeSyntax? baseType, IEnumerable<MemberDeclarationSyntax> members) GenerateComparable(string actualTypeFullName, string identifier)
        {
            var baseType = SimpleBaseType(
                                GenericName(
                                        Identifier("IComparable"))
                                    .WithTypeArgumentList(
                                        TypeArgumentList(
                                            SingletonSeparatedList<TypeSyntax>(
                                                IdentifierName(identifier)))));
            var members = 
                    List(
                        new MemberDeclarationSyntax[]
                        {
                            MethodDeclaration(
                                    PredefinedType(
                                        Token(SyntaxKind.IntKeyword)),
                                    Identifier("CompareTo"))
                                .WithModifiers(
                                    TokenList(
                                        Token(SyntaxKind.PublicKeyword)))
                                .WithParameterList(
                                    ParameterList(
                                        SingletonSeparatedList(
                                            Parameter(
                                                    Identifier("other"))
                                                .WithType(
                                                    IdentifierName(identifier)))))
                                .WithBody(
                                    Block(
                                        SingletonList<StatementSyntax>(
                                            ReturnStatement(
                                                InvocationExpression(
                                                        MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            IdentifierName("_value"),
                                                            IdentifierName("CompareTo")))
                                                    .WithArgumentList(
                                                        ArgumentList(
                                                            SingletonSeparatedList(
                                                                Argument(
                                                                    MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        IdentifierName("other"),
                                                                        IdentifierName("_value"))))))))))
                        });
            return (baseType, members);
        }
    }
}
