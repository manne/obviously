using System.Collections.Immutable;
using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generator.Modules
{
    internal static class JsonNetConverter
    {
        private const string ConverterName = "JsonNetConverter";

        internal static ImmutableArray<ClassDeclarationSyntax> Generate(TypedConstant actualType,
            TransformationContext context, SemanticTypeGenerator.Input input)
        {
            var hasNewtonsoftJson = context.SemanticModel.Compilation.HasType("Newtonsoft.Json.JsonConverter`1");
            if (!hasNewtonsoftJson)
            {
                return ImmutableArray<ClassDeclarationSyntax>.Empty;
            }

            var @class = ClassDeclaration(ConverterName)
                .WithModifiers(
                    TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.SealedKeyword)))
                .WithBaseList(
                    BaseList(
                        SingletonSeparatedList<BaseTypeSyntax>(
                            SimpleBaseType(
                                QualifiedName(
                                    QualifiedName(
                                        AliasQualifiedName(
                                            IdentifierName(
                                                Token(SyntaxKind.GlobalKeyword)),
                                            IdentifierName("Newtonsoft")),
                                        IdentifierName("Json")),
                                    GenericName(
                                        Identifier("JsonConverter"))
                                    .WithTypeArgumentList(
                                        TypeArgumentList(
                                            SingletonSeparatedList<TypeSyntax>(
                                                IdentifierName(input.Identifier)))))))))
                .WithMembers(
                    List(
                        new MemberDeclarationSyntax[]
                        {
                            MethodDeclaration(
                                PredefinedType(
                                    Token(SyntaxKind.VoidKeyword)),
                                Identifier("WriteJson"))
                            .WithModifiers(
                                TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.OverrideKeyword)))
                            .WithParameterList(
                                ParameterList(
                                    SeparatedList<ParameterSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            Parameter(
                                                Identifier("writer"))
                                            .WithType(
                                                QualifiedName(
                                                    QualifiedName(
                                                        AliasQualifiedName(
                                                            IdentifierName(
                                                                Token(SyntaxKind.GlobalKeyword)),
                                                            IdentifierName("Newtonsoft")),
                                                        IdentifierName("Json")),
                                                    IdentifierName("JsonWriter"))),
                                            Token(SyntaxKind.CommaToken),
                                            Parameter(
                                                Identifier("value"))
                                            .WithType(
                                                IdentifierName(input.Identifier)),
                                            Token(SyntaxKind.CommaToken),
                                            Parameter(
                                                Identifier("serializer"))
                                            .WithType(
                                                QualifiedName(
                                                    QualifiedName(
                                                        AliasQualifiedName(
                                                            IdentifierName(
                                                                Token(SyntaxKind.GlobalKeyword)),
                                                            IdentifierName("Newtonsoft")),
                                                        IdentifierName("Json")),
                                                    IdentifierName("JsonSerializer")))
                                        })))
            .WithBody(
                Block(
                    IfStatement(
                        IsPatternExpression(
                            IdentifierName("writer"),
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
                                                            IdentifierName("writer"))))))))))),
                    IfStatement(
                        IsPatternExpression(
                            IdentifierName("value"),
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
                                                            IdentifierName("value"))))))))))),
                    IfStatement(
                        IsPatternExpression(
                            IdentifierName("serializer"),
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
                                                            IdentifierName("serializer"))))))))))),
                    ExpressionStatement(
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName("serializer"),
                                IdentifierName("Serialize")))
                        .WithArgumentList(
                            ArgumentList(
                                SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]{
                                        Argument(
                                            IdentifierName("writer")),
                                        Token(SyntaxKind.CommaToken),
                                        Argument(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName("value"),
                                                IdentifierName("_value")))})))))),
            MethodDeclaration(
                IdentifierName(input.Identifier),
                Identifier("ReadJson"))
            .WithModifiers(
                TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.OverrideKeyword)))
            .WithParameterList(
                ParameterList(
                    SeparatedList<ParameterSyntax>(
                        new SyntaxNodeOrToken[]
                        {
                            Parameter(
                                Identifier("reader"))
                            .WithType(
                                QualifiedName(
                                    QualifiedName(
                                        AliasQualifiedName(
                                            IdentifierName(
                                                Token(SyntaxKind.GlobalKeyword)),
                                            IdentifierName("Newtonsoft")),
                                        IdentifierName("Json")),
                                    IdentifierName("JsonReader"))),
                            Token(SyntaxKind.CommaToken),
                            Parameter(
                                Identifier("objectType"))
                            .WithType(
                                QualifiedName(
                                    AliasQualifiedName(
                                        IdentifierName(
                                            Token(SyntaxKind.GlobalKeyword)),
                                        IdentifierName("System")),
                                    IdentifierName("Type"))),
                            Token(SyntaxKind.CommaToken),
                            Parameter(
                                Identifier("existingValue"))
                            .WithType(
                                IdentifierName(input.Identifier)),
                            Token(SyntaxKind.CommaToken),
                            Parameter(
                                Identifier("hasExistingValue"))
                            .WithType(
                                PredefinedType(
                                    Token(SyntaxKind.BoolKeyword))),
                            Token(SyntaxKind.CommaToken),
                            Parameter(
                                Identifier("serializer"))
                            .WithType(
                                QualifiedName(
                                    QualifiedName(
                                        AliasQualifiedName(
                                            IdentifierName(
                                                Token(SyntaxKind.GlobalKeyword)),
                                            IdentifierName("Newtonsoft")),
                                        IdentifierName("Json")),
                                    IdentifierName("JsonSerializer")))
                        })))
            .WithBody(
                Block(
                    IfStatement(
                        IsPatternExpression(
                            IdentifierName("serializer"),
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
                                                            IdentifierName("serializer"))))))))))),
                    LocalDeclarationStatement(
                        VariableDeclaration(
                            IdentifierName("var"))
                        .WithVariables(
                            SingletonSeparatedList(
                                VariableDeclarator(
                                    Identifier("deserialize"))
                                .WithInitializer(
                                    EqualsValueClause(
                                        InvocationExpression(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName("serializer"),
                                                GenericName(
                                                    Identifier("Deserialize"))
                                                .WithTypeArgumentList(
                                                    TypeArgumentList(
                                                        SingletonSeparatedList<TypeSyntax>(
                                                            IdentifierName(actualType.Value!.ToString()))))))
                                        .WithArgumentList(
                                            ArgumentList(
                                                SingletonSeparatedList(
                                                    Argument(
                                                        IdentifierName("reader")))))))))),
                    CreatePossibleIfForNullSerialization(input),
                    ReturnStatement(
                        ObjectCreationExpression(
                            IdentifierName(input.Identifier))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList(
                                    Argument(
                                        IdentifierName("deserialize"))))))))
                        }));
            return ImmutableArray.CreateRange(new[] { @class });
        }

        private static StatementSyntax CreatePossibleIfForNullSerialization(SemanticTypeGenerator.Input input)
        {
            StatementSyntax result;
            if (input.IsValueType)
            {
                result = EmptyStatement();
            }
            else
            {
                result = IfStatement(
                    IsPatternExpression(
                        IdentifierName("deserialize"),
                        ConstantPattern(
                            LiteralExpression(
                                SyntaxKind.NullLiteralExpression))),
                    Block(
                        SingletonList<StatementSyntax>(
                            ThrowStatement(
                                ObjectCreationExpression(
                                        QualifiedName(
                                            AliasQualifiedName(
                                                IdentifierName(
                                                    Token(SyntaxKind.GlobalKeyword)),
                                                IdentifierName("System")),
                                            IdentifierName("InvalidOperationException")))
                                    .WithArgumentList(
                                        ArgumentList(
                                            SingletonSeparatedList(
                                                Argument(
                                                    LiteralExpression(
                                                        SyntaxKind.StringLiteralExpression,
                                                        Literal("The deserialized object is null"))))))))));
            }

            return result;
        }
    }
}
