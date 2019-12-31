using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        private class Input
        {
            public Input(string actualTypeFullName, string identifier, ClassDeclarationSyntax applyToClass)
            {
                ActualTypeFullName = actualTypeFullName;
                Identifier = identifier;
                ApplyToClass = applyToClass;
            }

            public string ActualTypeFullName { get;  }

            public string Identifier { get; }

            public ClassDeclarationSyntax ApplyToClass { get; }
        }

        private class Output
        {
            public Output(SimpleBaseTypeSyntax? baseType, IImmutableList<MemberDeclarationSyntax> members)
            {
                BaseType = baseType;
                Members = members;
            }

            public SimpleBaseTypeSyntax? BaseType { get; }
            public IImmutableList<MemberDeclarationSyntax> Members { get; }
        }

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
                GenerateEquality,
                GenerateExplicitOperator,
                GenerateToString
            };
            var baseTypes = new List<SimpleBaseTypeSyntax>();
            var members = new List<MemberDeclarationSyntax>();
            var input = new Input(_actualTypeFullName, idName, applyToClass);
            foreach (var generator in generators)
            {
                var output = generator(input);
                if (output.BaseType != null)
                {
                    baseTypes.Add(output.BaseType);
                }

                members.AddRange(output.Members);
            }

            var result = SingletonList<MemberDeclarationSyntax>(
                ClassDeclaration(applyToClass.Identifier.ValueText)
                    .WithBaseList(BaseList(SeparatedList<BaseTypeSyntax>(baseTypes)))
                    .WithModifiers(
                        TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.PartialKeyword)))
                    .WithMembers(
                        List(members)));
            var wrappedMembers = result.WrapWithAncestors(context.ProcessingNode);

            return Task.FromResult(new RichGenerationResult
            {
                Members = new SyntaxList<MemberDeclarationSyntax>(wrappedMembers)
            });
        }

        private delegate Output Generate(Input input);
    }
}
