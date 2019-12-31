using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generator
{
    // ReSharper disable once UnusedMember.Global, reason: utilized by the roslyn code generator
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
                                    TokenList(
                                        new []{
                                            Token(SyntaxKind.PublicKeyword),
                                            Token(SyntaxKind.OverrideKeyword)}))
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
            return new Output(null, ImmutableList.CreateRange(members));
        }
    }
}
