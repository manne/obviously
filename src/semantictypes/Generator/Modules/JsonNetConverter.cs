﻿using System.Collections.Immutable;
using System.IO;
using System.Linq;
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

        internal static ImmutableArray<ClassDeclarationSyntax> Generate(TypedConstant actualType, TransformationContext context, string identifierName)
        {
            var hasNewtonsoftJson = context.SemanticModel.Compilation.ExternalReferences
                .Select(er => er.Display)
                .Select(Path.GetFileNameWithoutExtension)
                .FirstOrDefault(assemblyName => assemblyName == "Newtonsoft.Json");
            if (hasNewtonsoftJson is null)
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
                                                    IdentifierName(identifierName)))))))))
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
                                                        IdentifierName(identifierName)),
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
                                    IdentifierName(identifierName),
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
                                                        IdentifierName(identifierName)),
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
                                                                                                IdentifierName(actualType.Value.ToString()))))))
                                                                        .WithArgumentList(
                                                                            ArgumentList(
                                                                                SingletonSeparatedList(
                                                                                    Argument(
                                                                                        IdentifierName("reader")))))))))),
                                        ReturnStatement(
                                            ObjectCreationExpression(
                                                    IdentifierName(identifierName))
                                                .WithArgumentList(
                                                    ArgumentList(
                                                        SingletonSeparatedList(
                                                            Argument(
                                                                IdentifierName("deserialize"))))))))
                        }));
            return ImmutableArray.CreateRange(new[] { @class });
        }
    }
}
