using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generator
{
    // ReSharper disable once UnusedMember.Global, reason: utilized by the roslyn code generator
    public partial class SemanticTypeGenerator
    {
        private static (SimpleBaseTypeSyntax? baseType, IEnumerable<MemberDeclarationSyntax> members) GenerateConstructorAndField(string actualTypeFullName, string identifier)
        {
            var members = new MemberDeclarationSyntax[]
            {
                FieldDeclaration(
                        VariableDeclaration(IdentifierName(actualTypeFullName))
                            .WithVariables(
                                SingletonSeparatedList(
                                    VariableDeclarator(
                                        Identifier("_value")))))
                    .WithModifiers(
                        TokenList(
                            new[]
                            {
                                Token(SyntaxKind.PrivateKeyword),
                                Token(SyntaxKind.ReadOnlyKeyword)
                            })),
                ConstructorDeclaration(
                        Identifier(identifier))
                    .WithModifiers(
                        TokenList(
                            Token(SyntaxKind.PublicKeyword)))
                    .WithParameterList(
                        ParameterList(
                            SingletonSeparatedList(
                                Parameter(
                                        Identifier("value"))
                                    .WithType(IdentifierName(actualTypeFullName)))))
                    .WithBody(
                        Block(
                            SingletonList<StatementSyntax>(
                                ExpressionStatement(
                                    AssignmentExpression(
                                        SyntaxKind.SimpleAssignmentExpression,
                                        IdentifierName("_value"),
                                        IdentifierName("value"))))))

            };
            return (null, members);
        }
    }
}
