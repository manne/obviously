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
            static ExpressionSyntax ReturnExpression(Input i)
            {
                ExpressionSyntax r;
                var isNullable = !i.IsValueType && i.IsNullableType;
                if (isNullable)
                {
                    r = BinaryExpression(
                        SyntaxKind.CoalesceExpression,
                        ConditionalAccessExpression(
                            IdentifierName("_value"),
                            InvocationExpression(
                                MemberBindingExpression(
                                    IdentifierName("ToString")))),
                        LiteralExpression(
                            SyntaxKind.StringLiteralExpression,
                            Literal("")));
                }
                else
                {
                    r = InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(BackingFieldName),
                            IdentifierName("ToString")));
                }

                return r;
            }
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
                                            ReturnStatement(ReturnExpression(input)
                                                ))))
						});
            return new Output(ImmutableList.CreateRange(members));
        }
    }
}
