using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generator
{
    // ReSharper disable once UnusedMember.Global, reason: utilized by the roslyn code generator
    public partial class SemanticTypeGenerator
    {
        private static Output GenerateExplicitOperator(Input input)
        {
            var members = List(
                new MemberDeclarationSyntax[]
                {
                    ConversionOperatorDeclaration(
                    Token(SyntaxKind.ExplicitKeyword),
                    IdentifierName(input.ActualTypeFullName))
                    .WithModifiers(
                        TokenList(
                            new[]
                            {
                                Token(SyntaxKind.PublicKeyword),
                                Token(SyntaxKind.StaticKeyword)
                            }))
                    .WithParameterList(
                        ParameterList(
                            SingletonSeparatedList<ParameterSyntax>(
                                Parameter(
                                        Identifier("t"))
                                    .WithType(
                                        IdentifierName(input.Identifier)))))
                    .WithBody(
                        Block(
                            SingletonList<StatementSyntax>(
                                ReturnStatement(
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName("t"),
                                        IdentifierName(BackingFieldName))))))});
            return new Output(null, ImmutableList.CreateRange(members));
        }
    }
}
