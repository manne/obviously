using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generator
{
    public class SemanticTypeGenerator : IRichCodeGenerator
    {
        private readonly string _actualTypeFullName;
        public SemanticTypeGenerator(AttributeData attributeData)
        {
            if (attributeData is null) throw new ArgumentNullException(nameof(attributeData));

            var ca = attributeData.ConstructorArguments[0];
            _actualTypeFullName = ca.Value.ToString();
        }

        public Task<SyntaxList<MemberDeclarationSyntax>> GenerateAsync(TransformationContext context, IProgress<Diagnostic> progress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<RichGenerationResult> GenerateRichAsync(TransformationContext context, IProgress<Diagnostic> progress, CancellationToken cancellationToken)
        {
            var applyToClass = (ClassDeclarationSyntax)context.ProcessingNode;
            var idName = applyToClass.Identifier.ValueText;

            var generators = new Generate[] { GenerateConstructorAndField, GenerateIComparable};
            var baseTypes = new List<SimpleBaseTypeSyntax>();
            var members = new List<MemberDeclarationSyntax>();
            foreach (var generator in generators)
            {
                var (compBaseType, compMembers) = generator(_actualTypeFullName,idName);
                if (compBaseType != null)
                {
                    baseTypes.Add(compBaseType);
                }

                members.AddRange(compMembers);
            }

            var result = SingletonList<MemberDeclarationSyntax>(
                ClassDeclaration(applyToClass.Identifier.ValueText)
                    .WithBaseList(BaseList(SeparatedList<BaseTypeSyntax>(baseTypes)))
                    .WithModifiers(
                        TokenList(
                            new[]
                            {
                                Token(SyntaxKind.PublicKeyword),
                                Token(SyntaxKind.SealedKeyword),
                                Token(SyntaxKind.PartialKeyword)
                            }))
                    .WithMembers(
                        List<MemberDeclarationSyntax>(members)));
            var wrappedMembers = result.WrapWithAncestors(context.ProcessingNode);
            
            
            return Task.FromResult(new RichGenerationResult
            {
                Members = new SyntaxList<MemberDeclarationSyntax>(wrappedMembers)
            });
        }

        private delegate (SimpleBaseTypeSyntax? baseType, IEnumerable<MemberDeclarationSyntax> members) Generate(string actualTypeFullName, string identifier);

        private static (SimpleBaseTypeSyntax? baseType, IEnumerable<MemberDeclarationSyntax> members) GenerateConstructorAndField(string actualTypeFullName, string identifier)
        {
            var members = new MemberDeclarationSyntax[]
            {
                FieldDeclaration(
                        VariableDeclaration(IdentifierName(actualTypeFullName))
                            .WithVariables(
                                SingletonSeparatedList<VariableDeclaratorSyntax>(
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
                        Identifier(identifier))
                    .WithModifiers(
                        TokenList(
                            Token(SyntaxKind.PublicKeyword)))
                    .WithParameterList(
                        ParameterList(
                            SingletonSeparatedList<ParameterSyntax>(
                                Parameter(
                                        Identifier("value"))
                                    .WithType(IdentifierName(actualTypeFullName)))))
                    .WithBody(
                        Block(
                            SingletonList<StatementSyntax>(
                                ExpressionStatement(
                                    AssignmentExpression(
                                        SyntaxKind.SimpleAssignmentExpression,
                                        IdentifierName("_value"),
                                        IdentifierName("value"))))))

            };
            return (null, members);
        }

        private static (SimpleBaseTypeSyntax? baseType, IEnumerable<MemberDeclarationSyntax> members) GenerateIComparable(string actualTypeFullName, string identifier)
        {
            var baseType = SimpleBaseType(
                                GenericName(
                                        Identifier("IComparable"))
                                    .WithTypeArgumentList(
                                        TypeArgumentList(
                                            SingletonSeparatedList<TypeSyntax>(
                                                IdentifierName(identifier)))));
            var members = 
                    List<MemberDeclarationSyntax>(
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
                                        SingletonSeparatedList<ParameterSyntax>(
                                            Parameter(
                                                    Identifier("other"))
                                                .WithType(
                                                    IdentifierName(identifier)))))
                                .WithBody(
                                    Block(
                                        SingletonList<StatementSyntax>(
                                            ReturnStatement(
                                                InvocationExpression(
                                                        MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            IdentifierName("_value"),
                                                            IdentifierName("CompareTo")))
                                                    .WithArgumentList(
                                                        ArgumentList(
                                                            SingletonSeparatedList<ArgumentSyntax>(
                                                                Argument(
                                                                    MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        IdentifierName("other"),
                                                                        IdentifierName("_value"))))))))))
                        });
            return (baseType, members);
        }
    }
}
