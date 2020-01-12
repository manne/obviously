using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generator.Modules
{
    public partial class AspNetCoreModelBinding
    {
        private static ClassDeclarationSyntax ConstructProvider(string identifierName)
        {
            var binderProvider = ClassDeclaration(identifierName + "ModelBinderProvider")
                .WithModifiers(
                    TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.SealedKeyword)))
                .WithBaseList(
                    BaseList(
                        SingletonSeparatedList<BaseTypeSyntax>(
                            SimpleBaseType(
                                QualifiedName(
                                    QualifiedName(
                                        QualifiedName(
                                            QualifiedName(
                                                AliasQualifiedName(
                                                    IdentifierName(
                                                        Token(SyntaxKind.GlobalKeyword)),
                                                    IdentifierName("Microsoft")),
                                                IdentifierName("AspNetCore")),
                                            IdentifierName("Mvc")),
                                        IdentifierName("ModelBinding")),
                                    IdentifierName("IModelBinderProvider"))))))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        MethodDeclaration(
                                QualifiedName(
                                    QualifiedName(
                                        QualifiedName(
                                            QualifiedName(
                                                AliasQualifiedName(
                                                    IdentifierName(
                                                        Token(SyntaxKind.GlobalKeyword)),
                                                    IdentifierName("Microsoft")),
                                                IdentifierName("AspNetCore")),
                                            IdentifierName("Mvc")),
                                        IdentifierName("ModelBinding")),
                                    IdentifierName("IModelBinder")),
                                Identifier("GetBinder"))
                            .WithModifiers(
                                TokenList(
                                    Token(SyntaxKind.PublicKeyword)))
                            .WithParameterList(
                                ParameterList(
                                    SingletonSeparatedList(
                                        Parameter(
                                                Identifier("context"))
                                            .WithType(
                                                QualifiedName(
                                                    QualifiedName(
                                                        QualifiedName(
                                                            QualifiedName(
                                                                AliasQualifiedName(
                                                                    IdentifierName(
                                                                        Token(SyntaxKind.GlobalKeyword)),
                                                                    IdentifierName("Microsoft")),
                                                                IdentifierName("AspNetCore")),
                                                            IdentifierName("Mvc")),
                                                        IdentifierName("ModelBinding")),
                                                    IdentifierName("ModelBinderProviderContext"))))))
                            .WithBody(
                                Block(
                                    IfStatement(
                                        IsPatternExpression(
                                            IdentifierName("context"),
                                            ConstantPattern(
                                                LiteralExpression(
                                                    SyntaxKind.NullLiteralExpression))),
                                        ThrowStatement(
                                            ObjectCreationExpression(
                                                    QualifiedName(
                                                        AliasQualifiedName(
                                                            IdentifierName(
                                                                Token(SyntaxKind.GlobalKeyword)),
                                                            IdentifierName("System")),
                                                        IdentifierName("ArgumentNullException")))
                                                .WithArgumentList(
                                                    ArgumentList(
                                                        SingletonSeparatedList(
                                                            Argument(
                                                                InvocationExpression(
                                                                        IdentifierName("nameof"))
                                                                    .WithArgumentList(
                                                                        ArgumentList(
                                                                            SingletonSeparatedList(
                                                                                Argument(
                                                                                    IdentifierName("context"))))))))))),
                                    ReturnStatement(
                                        ConditionalExpression(
                                            BinaryExpression(
                                                SyntaxKind.EqualsExpression,
                                                MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        IdentifierName("context"),
                                                        IdentifierName("Metadata")),
                                                    IdentifierName("ModelType")),
                                                TypeOfExpression(
                                                    IdentifierName(identifierName))),
                                            ObjectCreationExpression(
                                                    IdentifierName(identifierName + "ModelBinder"))
                                                .WithArgumentList(
                                                    ArgumentList()),
                                            LiteralExpression(
                                                SyntaxKind.NullLiteralExpression)))))));

            return binderProvider;

        }
    }
}
