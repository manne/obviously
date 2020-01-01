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
        private const string BackingFieldName = "_value";
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

        private delegate Output Generate(Input input);

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
            if (context is null) throw new ArgumentNullException(nameof(context));

            var applyToClass = (ClassDeclarationSyntax)context.ProcessingNode;
            var classSymbol = context.SemanticModel.GetDeclaredSymbol(applyToClass);
            var idName = classSymbol.Name;

            var generators = new Generate[]
            {
                GenerateConstructorAndField,
                GenerateComparable,
                GenerateEquality,
                GenerateExplicitOperator,
                GenerateToString
            };
            var accessibility = classSymbol.DeclaredAccessibility;
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
                        TokenList( ParseToken(SyntaxFacts.GetText(accessibility)), Token(SyntaxKind.PartialKeyword)))
                    .WithMembers(
                        List(members)));
            var wrappedMembers = result.WrapWithAncestors(context.ProcessingNode);

            return Task.FromResult(new RichGenerationResult
            {
                Members = new SyntaxList<MemberDeclarationSyntax>(wrappedMembers)
            });
        }
    }
}
