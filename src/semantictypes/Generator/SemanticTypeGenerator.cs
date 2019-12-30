using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
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
            //Logger.Warning($"aa {ca.Value}* {ca.Type.GetType().FullName}", "MPF0001");
            //_actualType = (PENamedTypeSymbolNonGeneric)attributeData.ConstructorArguments[0].Value;
        }

        public Task<SyntaxList<MemberDeclarationSyntax>> GenerateAsync(TransformationContext context, IProgress<Diagnostic> progress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<RichGenerationResult> GenerateRichAsync(TransformationContext context, IProgress<Diagnostic> progress, CancellationToken cancellationToken)
        {
            var applyToClass = (ClassDeclarationSyntax)context.ProcessingNode;
            var idName = applyToClass.Identifier.ValueText;
            var members = SingletonList<MemberDeclarationSyntax>(
                    ClassDeclaration(applyToClass.Identifier.ValueText)
                        .WithModifiers(
                            TokenList(
                                new[]
                                {
                                    Token(SyntaxKind.PublicKeyword),
                                    Token(SyntaxKind.SealedKeyword),
                                    Token(SyntaxKind.PartialKeyword)
                                }))
                        .WithMembers(
                            List<MemberDeclarationSyntax>(
                                new MemberDeclarationSyntax[]{
                                    FieldDeclaration(
                                            VariableDeclaration(IdentifierName(_actualTypeFullName))
                                                .WithVariables(
                                                    SingletonSeparatedList<VariableDeclaratorSyntax>(
                                                        VariableDeclarator(
                                                            Identifier("_value")))))
                                        .WithModifiers(
                                            TokenList(
                                                new []{
                                                    Token(SyntaxKind.PrivateKeyword),
                                                    Token(SyntaxKind.ReadOnlyKeyword)})),
                                    ConstructorDeclaration(
                                            Identifier(idName))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.PublicKeyword)))
                                        .WithParameterList(
                                            ParameterList(
                                                SingletonSeparatedList<ParameterSyntax>(
                                                    Parameter(
                                                            Identifier("value"))
                                                        .WithType(IdentifierName(_actualTypeFullName)))))
                                        .WithBody(
                                            Block(
                                                SingletonList<StatementSyntax>(
                                                    ExpressionStatement(
                                                        AssignmentExpression(
                                                            SyntaxKind.SimpleAssignmentExpression,
                                                            IdentifierName("_value"),
                                                            IdentifierName("value"))))))}))
                );

            var wrappedMembers = members.WrapWithAncestors(context.ProcessingNode);

            return Task.FromResult(new RichGenerationResult
            {
                Members = wrappedMembers
            });
        }
    }
}
