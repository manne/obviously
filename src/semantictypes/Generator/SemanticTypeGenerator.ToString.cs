using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generator
{
    public partial class SemanticTypeGenerator
    {
        private static Output GenerateToString(Input input)
        {
            var members =
                    List(
                        new MemberDeclarationSyntax[]
                        {
							MethodDeclaration(
                                    PredefinedType(
                                        Token(SyntaxKind.StringKeyword)),
                                    Identifier("ToString"))
                                .WithModifiers(
                                    TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.OverrideKeyword)))
                                .WithBody(
                                    Block(
                                        SingletonList<StatementSyntax>(
                                            ReturnStatement(
                                                InvocationExpression(
                                                    MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        IdentifierName(BackingFieldName),
                                                        IdentifierName("ToString")))))))
						});
            return new Output(ImmutableList.CreateRange(members));
        }
    }
}
