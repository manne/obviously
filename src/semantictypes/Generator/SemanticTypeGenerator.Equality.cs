using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generator
{
    public partial class SemanticTypeGenerator
    {
        private static Output GenerateEquality(Input input)
        {
            static ExpressionSyntax CreateGetHashCode(Input i)
            {
                ExpressionSyntax r;
                if (i.IsRelevantForNullability())
                {
                    r = BinaryExpression(
                        SyntaxKind.CoalesceExpression,
                        ConditionalAccessExpression(
                            IdentifierName(BackingFieldName),
                            InvocationExpression(
                                MemberBindingExpression(
                                    IdentifierName("GetHashCode")))),
                        LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(0)));
                }
                else
                {
                    r = InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(BackingFieldName),
                            IdentifierName("GetHashCode")));
                }

                return r;
            }
            var baseType = SimpleBaseType(
                GenericName(
                        Identifier("global::System.IEquatable"))
                    .WithTypeArgumentList(
                        TypeArgumentList(
                            SingletonSeparatedList(
                                IdentifierName(input.Identifier).MakeNullableIfEnabled(input)))));
            var members =
                List(
                    new MemberDeclarationSyntax[]
                    {
                        MethodDeclaration(
                                PredefinedType(
                                    Token(SyntaxKind.BoolKeyword)),
                                Identifier("Equals"))
                            .WithModifiers(
                                TokenList(
                                    Token(SyntaxKind.PublicKeyword)))
                            .WithParameterList(
                                ParameterList(
                                    SingletonSeparatedList(
                                        Parameter(
                                                Identifier("other"))
                                            .WithType(
                                                IdentifierName(input.Identifier).MakeNullableIfEnabled(input)))))
                            .WithBody(
                                Block(
                                    IfStatement(
                                        IsPatternExpression(
                                            IdentifierName("other"),
                                            ConstantPattern(
                                                LiteralExpression(
                                                    SyntaxKind.NullLiteralExpression))),
                                        ReturnStatement(
                                            LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))),
                                    IfStatement(
                                        InvocationExpression(
                                                IdentifierName("ReferenceEquals"))
                                            .WithArgumentList(
                                                ArgumentList(
                                                    SeparatedList<ArgumentSyntax>(
                                                        new SyntaxNodeOrToken[]
                                                        {
                                                            Argument(
                                                                ThisExpression()),
                                                            Token(SyntaxKind.CommaToken),
                                                            Argument(
                                                                IdentifierName("other"))
                                                        }))),
                                        ReturnStatement(
                                            LiteralExpression(
                                                SyntaxKind.TrueLiteralExpression))),
                                    ReturnStatement(
                                        BinaryExpression(
                                            SyntaxKind.EqualsExpression,
                                            IdentifierName(BackingFieldName),
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName("other"),
                                                IdentifierName(BackingFieldName)))))),
                        MethodDeclaration(
                                PredefinedType(
                                    Token(SyntaxKind.BoolKeyword)),
                                Identifier("Equals"))
                            .WithModifiers(
                                TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.OverrideKeyword)))
                            .WithParameterList(
                                ParameterList(
                                    SingletonSeparatedList(
                                        Parameter(
                                            Identifier("other"))
                                        .WithType(
                                            PredefinedType(
                                                Token(SyntaxKind.ObjectKeyword)).MakeNullableIfEnabled(input)))))
                            .WithBody(
                                Block(
                                    IfStatement(
                                        InvocationExpression(
                                            IdentifierName("ReferenceEquals"))
                                        .WithArgumentList(
                                            ArgumentList(
                                                SeparatedList<ArgumentSyntax>(
                                                    new SyntaxNodeOrToken[]{
                                                        Argument(
                                                            ThisExpression()),
                                                        Token(SyntaxKind.CommaToken),
                                                        Argument(
                                                            IdentifierName("other"))}))),
                                        Block(
                                            SingletonList<StatementSyntax>(
                                                ReturnStatement(
                                                    LiteralExpression(
                                                        SyntaxKind.TrueLiteralExpression))))),
                                    IfStatement(
                                        PrefixUnaryExpression(
                                            SyntaxKind.LogicalNotExpression,
                                            ParenthesizedExpression(
                                                IsPatternExpression(
                                                    IdentifierName("other"),
                                                    DeclarationPattern(
                                                        IdentifierName(input.Identifier),
                                                        SingleVariableDesignation(
                                                            Identifier("other2")))))),
                                        Block(
                                            SingletonList<StatementSyntax>(
                                                ReturnStatement(
                                                    LiteralExpression(
                                                        SyntaxKind.FalseLiteralExpression))))),
                                    ReturnStatement(
                                        BinaryExpression(
                                            SyntaxKind.EqualsExpression,
                                            IdentifierName("_value"),
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName("other2"),
                                                IdentifierName("_value")))))),
                        MethodDeclaration(
                                PredefinedType(
                                    Token(SyntaxKind.IntKeyword)),
                                Identifier("GetHashCode"))
                            .WithModifiers(
                                TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.OverrideKeyword)))
                            .WithBody(
                                Block(
                                    SingletonList<StatementSyntax>(
                                        ReturnStatement(CreateGetHashCode(input)
                                            )))),
                        OperatorDeclaration(
                                PredefinedType(
                                    Token(SyntaxKind.BoolKeyword)),
                                Token(SyntaxKind.EqualsEqualsToken))
                            .WithModifiers(
                                TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
                            .WithParameterList(
                                ParameterList(
                                    SeparatedList<ParameterSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            Parameter(
                                                    Identifier("left"))
                                                .WithType(
                                                    IdentifierName(input.Identifier).MakeNullableIfEnabled(input)),
                                            Token(SyntaxKind.CommaToken),
                                            Parameter(
                                                    Identifier("right"))
                                                .WithType(
                                                    IdentifierName(input.Identifier).MakeNullableIfEnabled(input))
                                        })))
                            .WithBody(
                                Block(
                                    SingletonList<StatementSyntax>(
                                        ReturnStatement(
                                            InvocationExpression(
                                                    IdentifierName("Equals"))
                                                .WithArgumentList(
                                                    ArgumentList(
                                                        SeparatedList<ArgumentSyntax>(
                                                            new SyntaxNodeOrToken[]
                                                            {
                                                                Argument(
                                                                    IdentifierName("left")),
                                                                Token(SyntaxKind.CommaToken),
                                                                Argument(
                                                                    IdentifierName("right"))
                                                            }))))))),
                        OperatorDeclaration(
                                PredefinedType(
                                    Token(SyntaxKind.BoolKeyword)),
                                Token(SyntaxKind.ExclamationEqualsToken))
                            .WithModifiers(
                                TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
                            .WithParameterList(
                                ParameterList(
                                    SeparatedList<ParameterSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            Parameter(
                                                    Identifier("left"))
                                                .WithType(
                                                    IdentifierName(input.Identifier).MakeNullableIfEnabled(input)),
                                            Token(SyntaxKind.CommaToken),
                                            Parameter(
                                                    Identifier("right"))
                                                .WithType(
                                                    IdentifierName(input.Identifier).MakeNullableIfEnabled(input))
                                        })))
                            .WithBody(
                                Block(
                                    SingletonList<StatementSyntax>(
                                        ReturnStatement(
                                            PrefixUnaryExpression(
                                                SyntaxKind.LogicalNotExpression,
                                                InvocationExpression(
                                                        IdentifierName("Equals"))
                                                    .WithArgumentList(
                                                        ArgumentList(
                                                            SeparatedList<ArgumentSyntax>(
                                                                new SyntaxNodeOrToken[]
                                                                {
                                                                    Argument(
                                                                        IdentifierName("left")),
                                                                    Token(SyntaxKind.CommaToken),
                                                                    Argument(
                                                                        IdentifierName("right"))
                                                                }))))))))
                    });
            return new Output(baseType, ImmutableList.CreateRange(members));
        }
    }
}
