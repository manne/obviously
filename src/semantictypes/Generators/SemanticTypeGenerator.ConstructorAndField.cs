using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generator
{
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
                    var parameterTypeName = parameter.Type!.ToFullString().Trim();
                    var actualTypeName = input.ActualTypeFullName;
                    if (parameterTypeName != actualTypeName)
                    {
                        Logger.Warning($"The semantic type {input.Identifier} has got an invalid validation method. The parameter type is incorrect, it is '{parameterTypeName}', but should be '{actualTypeName}'.", "OBVST004");
                    }
                }
            }

            var blockStatements = new List<StatementSyntax>();

            if (hasValidValidation)
            {
                var parameterValidation =
                    IfStatement(
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
                blockStatements.Add(parameterValidation);
            }

            var parameterAssignment = ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(BackingFieldName),
                    IdentifierName("value")));
            blockStatements.Add(parameterAssignment);

            var members = new MemberDeclarationSyntax[]
            {
                FieldDeclaration(
                        VariableDeclaration(IdentifierName(input.ActualTypeFullName).MakeNullableIfEnabledButNotIfStruct(input))
                            .WithVariables(
                                SingletonSeparatedList(
                                    VariableDeclarator(
                                        Identifier(BackingFieldName)))))
                    .WithModifiers(
                        TokenList(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.ReadOnlyKeyword))),
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
                                    .WithType(IdentifierName(input.ActualTypeFullName).MakeNullableIfEnabledButNotIfStruct(input)))))
                    .WithBody(
                        Block(blockStatements))
            };
            return new Output((BaseListSyntax)null!, ImmutableList.CreateRange(members));
        }
    }
}
