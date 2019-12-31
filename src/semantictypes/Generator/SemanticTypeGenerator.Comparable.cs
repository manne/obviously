using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generator
{
    // ReSharper disable once UnusedMember.Global, reason: utilized by the roslyn code generator
    public partial class SemanticTypeGenerator
    {
        private static Output GenerateComparable(Input input)
        {
            var baseType = SimpleBaseType(
                                GenericName(
                                        Identifier("global::System.IComparable"))
                                    .WithTypeArgumentList(
                                        TypeArgumentList(
                                            SingletonSeparatedList<TypeSyntax>(
                                                IdentifierName(input.Identifier)))));
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
                                                    IdentifierName(input.Identifier)))))
                                .WithBody(
                                    Block(
                                        SingletonList<StatementSyntax>(
                                            ReturnStatement(
                                                InvocationExpression(
                                                        MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            IdentifierName(BackingFieldName),
                                                            IdentifierName("CompareTo")))
                                                    .WithArgumentList(
                                                        ArgumentList(
                                                            SingletonSeparatedList(
                                                                Argument(
                                                                    MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        IdentifierName("other"),
                                                                        IdentifierName(BackingFieldName))))))))))
                        });
            return new Output(baseType, ImmutableList.CreateRange(members));
        }
    }
}
