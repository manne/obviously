using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.Exceptional.Generator
{
    internal sealed class ExceptionGenerator : ICodeGenerator
    {
        public ExceptionGenerator(AttributeData attributeData)
        {
            if (attributeData is null) throw new ArgumentNullException(nameof(attributeData));
        }

        public Task<SyntaxList<MemberDeclarationSyntax>> GenerateAsync(TransformationContext context,
            IProgress<Diagnostic> progress, CancellationToken cancellationToken)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            var applyToClass = (ClassDeclarationSyntax)context.ProcessingNode;
            var classSymbol = context.SemanticModel.GetDeclaredSymbol(applyToClass);
            var idName = classSymbol.Name;

            var propertyInfos = GetPropertyInfos(applyToClass);

            var members = new List<MemberDeclarationSyntax>(5)
            {
                GetConstructorDefault(idName),
                GetConstructorWithMessage(idName),
                GetConstructorWithMessageAndInnerException(idName),
                GetConstructorForSerialization(idName, propertyInfos),
                GetObjectDataOverride(propertyInfos)
            };

            var baseList = applyToClass.DescendantNodes().OfType<BaseListSyntax>().FirstOrDefault();
            var classWithoutBaseList = applyToClass.RemoveNode(baseList, SyntaxRemoveOptions.KeepNoTrivia);

            var finalClass = classWithoutBaseList
                .WithAttributeLists(new SyntaxList<AttributeListSyntax>())
                .WithMembers(new SyntaxList<MemberDeclarationSyntax>(members));
            return Task.FromResult(new SyntaxList<MemberDeclarationSyntax>(finalClass));
        }

        private static IImmutableDictionary<string, string> GetPropertyInfos(ClassDeclarationSyntax applyToClass)
        {
            var dictionary = new Dictionary<string, string>();
            var properties = applyToClass.DescendantNodes().OfType<PropertyDeclarationSyntax>();
            foreach (var property in properties)
            {
                var name = property.Identifier.ValueText;
                var typeSyntax = (PredefinedTypeSyntax)property.Type;
                dictionary.Add(name, typeSyntax.Keyword.ValueText);
            }

            return dictionary.ToImmutableDictionary();
        }

        private static MethodDeclarationSyntax GetObjectDataOverride(IImmutableDictionary<string, string> propertiesInfo)
        {
            var assignments = new List<ExpressionStatementSyntax>(propertiesInfo.Count + 1)
            {
                ExpressionStatement(
                    InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                BaseExpression(),
                                IdentifierName("GetObjectData")))
                        .WithArgumentList(
                            ArgumentList(
                                SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        Argument(
                                            IdentifierName("info")),
                                        Token(SyntaxKind.CommaToken),
                                        Argument(
                                            IdentifierName("context"))
                                    }))))
            };

            foreach (var (name, typeName) in propertiesInfo)
            {
                var assignment = GetPropertySerializationAssignment(name, typeName);
                assignments.Add(assignment);
            }

            var method = MethodDeclaration(
                    PredefinedType(
                        Token(SyntaxKind.VoidKeyword)),
                    Identifier("GetObjectData"))
                .WithModifiers(
                    TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.OverrideKeyword)))
                .WithParameterList(
                    ParameterList(
                        SeparatedList<ParameterSyntax>(
                            new SyntaxNodeOrToken[]{
                                Parameter(
                                        Identifier("info"))
                                    .WithType(
                                        IdentifierName("global::System.Runtime.Serialization.SerializationInfo")),
                                Token(SyntaxKind.CommaToken),
                                Parameter(
                                        Identifier("context"))
                                    .WithType(
                                        IdentifierName("global::System.Runtime.Serialization.StreamingContext"))})))
                .WithBody(Block(assignments));
            return method;
        }

        private static ConstructorDeclarationSyntax GetConstructorDefault(string idName)
        {
            var constructor = ConstructorDeclaration(
                    Identifier(idName))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithInitializer(
                    ConstructorInitializer(
                        SyntaxKind.BaseConstructorInitializer,
                        ArgumentList()))
                .WithBody(Block());
            return constructor;
        }

        private static ConstructorDeclarationSyntax GetConstructorWithMessage(string idName)
        {
            var constructor = ConstructorDeclaration(
                    Identifier(idName))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(
                    ParameterList(
                        SingletonSeparatedList(
                            Parameter(
                                    Identifier("message"))
                                .WithType(
                                    PredefinedType(
                                        Token(SyntaxKind.StringKeyword))))))
                .WithInitializer(
                    ConstructorInitializer(
                        SyntaxKind.BaseConstructorInitializer,
                        ArgumentList(
                            SingletonSeparatedList(
                                Argument(
                                    IdentifierName("message"))))))
                .WithBody(Block());
            return constructor;
        }

        private static ConstructorDeclarationSyntax GetConstructorWithMessageAndInnerException(string idName)
        {
            var constructor = ConstructorDeclaration(
                    Identifier(idName))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(
                    ParameterList(
                        SeparatedList<ParameterSyntax>(
                            new SyntaxNodeOrToken[]
                            {
                                Parameter(
                                        Identifier("message"))
                                    .WithType(
                                        PredefinedType(
                                            Token(SyntaxKind.StringKeyword))),
                                Token(SyntaxKind.CommaToken),
                                Parameter(
                                        Identifier("inner"))
                                    .WithType(
                                        IdentifierName("global::System.Exception"))
                            })))
                .WithInitializer(
                    ConstructorInitializer(
                        SyntaxKind.BaseConstructorInitializer,
                        ArgumentList(
                            SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    Argument(
                                        IdentifierName("message")),
                                    Token(SyntaxKind.CommaToken),
                                    Argument(
                                        IdentifierName("inner"))
                                }))))
                .WithBody(Block());
            return constructor;
        }

        private static ConstructorDeclarationSyntax GetConstructorForSerialization(string idName, IImmutableDictionary<string, string> propertiesInfo)
        {
            var propertyAssignments = new List<ExpressionStatementSyntax>(propertiesInfo.Count);
            foreach (var (name, typeName) in propertiesInfo)
            {
                var assignment = GetPropertyDeserializationAssignment(name, typeName);
                propertyAssignments.Add(assignment);
            }

            var constructor = ConstructorDeclaration(
                    Identifier(idName))
                .WithModifiers(
                    TokenList(
                        Token(
                            TriviaList(
                                Trivia(
                                    PragmaWarningDirectiveTrivia(
                                            Token(SyntaxKind.DisableKeyword),
                                            true)
                                        .WithErrorCodes(
                                            SingletonSeparatedList<ExpressionSyntax>(
                                                IdentifierName(
                                                    Identifier(
                                                        TriviaList(),
                                                        "CS0628",
                                                        TriviaList(
                                                            Comment("// New protected member declared in sealed class")))))))),
                            SyntaxKind.ProtectedKeyword, TriviaList())))
                .WithParameterList(
                    ParameterList(
                        SeparatedList<ParameterSyntax>(
                            new SyntaxNodeOrToken[]{
                                Parameter(
                                        Identifier("info"))
                                    .WithType(
                                        IdentifierName("global::System.Runtime.Serialization.SerializationInfo")),
                                Token(SyntaxKind.CommaToken),
                                Parameter(
                                        Identifier("context"))
                                    .WithType(
                                        IdentifierName("global::System.Runtime.Serialization.StreamingContext"))})))
                .WithInitializer(
                    ConstructorInitializer(
                            SyntaxKind.BaseConstructorInitializer,
                            ArgumentList(
                                SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]{
                                        Argument(
                                            IdentifierName("info")),
                                        Token(SyntaxKind.CommaToken),
                                        Argument(
                                            IdentifierName("context"))})))
                        .WithColonToken(
                            Token(
                                TriviaList(
                                    Trivia(
                                        PragmaWarningDirectiveTrivia(
                                                Token(SyntaxKind.RestoreKeyword),
                                                true)
                                            .WithErrorCodes(
                                                SingletonSeparatedList<ExpressionSyntax>(
                                                    IdentifierName(
                                                        Identifier(
                                                            TriviaList(),
                                                            "CS0628",
                                                            TriviaList(
                                                                Comment("// New protected member declared in sealed class")))))))),
                                SyntaxKind.ColonToken,
                                TriviaList())))
                .WithBody(Block(propertyAssignments));
            return constructor;
        }

        private static ExpressionStatementSyntax GetPropertyDeserializationAssignment(string name, string typeName)
        {
            var assignment = ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(name),
                    CastExpression(
                            IdentifierName(typeName),
                        InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("info"),
                                    IdentifierName("GetValue")))
                            .WithArgumentList(
                                ArgumentList(
                                    SeparatedList<ArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            Argument(
                                                LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    Literal(name))),
                                            Token(SyntaxKind.CommaToken),
                                            Argument(
                                                TypeOfExpression(IdentifierName(typeName)))
                                        }))))));
            return assignment;
        }

        private static ExpressionStatementSyntax GetPropertySerializationAssignment(string name, string typeName)
            => ExpressionStatement(
                InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("info"),
                            IdentifierName("AddValue")))
                    .WithArgumentList(
                        ArgumentList(
                            SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    Argument(
                                        LiteralExpression(
                                            SyntaxKind.StringLiteralExpression,
                                            Literal(name))),
                                    Token(SyntaxKind.CommaToken),
                                    Argument(
                                        IdentifierName(name)),
                                    Token(SyntaxKind.CommaToken),
                                    Argument(
                                        TypeOfExpression(IdentifierName(typeName)))
                                }))));
    }
}
