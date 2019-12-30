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
    // ReSharper disable once UnusedMember.Global, reason: utilized by the roslyn code generator
    public sealed partial class SemanticTypeGenerator : IRichCodeGenerator
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

            var generators = new Generate[]
            {
                GenerateConstructorAndField,
                GenerateComparable,
                GenerateEquality
            };
            var baseTypes = new List<SimpleBaseTypeSyntax>();
            var members = new List<MemberDeclarationSyntax>();
            foreach (var generator in generators)
            {
                var (compBaseType, compMembers) = generator(_actualTypeFullName, idName);
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
                        TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.SealedKeyword), Token(SyntaxKind.PartialKeyword)))
                    .WithMembers(
                        List(members)));
            var wrappedMembers = result.WrapWithAncestors(context.ProcessingNode);

            return Task.FromResult(new RichGenerationResult
            {
                Members = new SyntaxList<MemberDeclarationSyntax>(wrappedMembers)
            });
        }

        private delegate (SimpleBaseTypeSyntax? baseType, IEnumerable<MemberDeclarationSyntax> members) Generate(string actualTypeFullName, string identifier);
    }
}
