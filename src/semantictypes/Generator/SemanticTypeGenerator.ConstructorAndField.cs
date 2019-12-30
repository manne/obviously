﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generator
{
    // ReSharper disable once UnusedMember.Global, reason: utilized by the roslyn code generator
    public partial class SemanticTypeGenerator
    {
        private static Output GenerateConstructorAndField(Input input)
        {
            var validationMethods = input.ApplyToClass
                .DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Where(m => m.Identifier.Text == "IsValid").ToArray();

            if (validationMethods.Length > 1)
            {
                Logger.Error($"The semantic type {input.Identifier} has more than one validation method named 'IsValid'.", "OBVST001");
            }

            var validationMethod = validationMethods.FirstOrDefault();
            StatementSyntax parameterValidation;
            bool hasValidValidation;

            if (validationMethod is null)
            {
                hasValidValidation = false;
            }
            else
            {
                hasValidValidation = validationMethod.Modifiers.Any(modifier => modifier.ValueText == "static");

                if (!hasValidValidation)
                {
                    Logger.Warning($"The semantic type {input.Identifier} has got an invalid validation method. The method must be static.", "OBVST002");
                }

                var parameters = validationMethod.ParameterList.DescendantNodes().OfType<ParameterSyntax>().ToArray();
                if (parameters.Length != 1)
                {
                    Logger.Warning($"The semantic type {input.Identifier} has got an invalid validation method. The method has more than one parameter.", "OBVST003");
                }
                else
                {
                    var parameter = parameters[0];
                    var parameterTypeName = parameter.Type.ToFullString().Trim();
                    var actualTypeName = input.ActualTypeFullName;
                    if (parameterTypeName != actualTypeName)
                    {
                        Logger.Warning($"The semantic type {input.Identifier} has got an invalid validation method. The parameter type is incorrect, it is '{parameterTypeName}', but should be '{actualTypeName}'.", "OBVST004");
                    }
                }
            }

            if (hasValidValidation)
            {
                parameterValidation = IfStatement(
                    PrefixUnaryExpression(
                        SyntaxKind.LogicalNotExpression,
                        InvocationExpression(
                                IdentifierName("IsValid"))
                            .WithArgumentList(
                                ArgumentList(
                                    SingletonSeparatedList(
                                        Argument(
                                            IdentifierName("value")))))),
                    ThrowStatement(
                        ObjectCreationExpression(
                                IdentifierName("global::System.ArgumentException"))
                            .WithArgumentList(
                                ArgumentList(
                                    SeparatedList<ArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            Argument(
                                                LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    Literal("The parameter is invalid"))),
                                            Token(SyntaxKind.CommaToken),
                                            Argument(
                                                LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    Literal("value")))
                                        })))));
            }
            else
            {
                parameterValidation = EmptyStatement();
            }

            var members = new MemberDeclarationSyntax[]
            {
                FieldDeclaration(
                        VariableDeclaration(IdentifierName(input.ActualTypeFullName))
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
                        Identifier(input.Identifier))
                    .WithModifiers(
                        TokenList(
                            Token(SyntaxKind.PublicKeyword)))
                    .WithParameterList(
                        ParameterList(
                            SingletonSeparatedList(
                                Parameter(
                                        Identifier("value"))
                                    .WithType(IdentifierName(input.ActualTypeFullName)))))
                    .WithBody(
                        Block(
                            parameterValidation,
                            ExpressionStatement(
                                AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    IdentifierName("_value"),
                                    IdentifierName("value")))))

            };
            return new Output(null, ImmutableList.CreateRange(members));
        }
    }
}
