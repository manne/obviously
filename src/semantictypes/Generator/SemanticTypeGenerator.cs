using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Obviously.SemanticTypes.Generator.Modules;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generator
{
    public sealed partial class SemanticTypeGenerator : IRichCodeGenerator
    {
        public const string BackingFieldName = "_value";
        private readonly TypedConstant _actualType;

        private sealed class Input
        {
            public Input(string actualTypeFullName, string identifier, ClassDeclarationSyntax applyToClass, bool isNullableEnabled)
            {
                ActualTypeFullName = actualTypeFullName;
                Identifier = identifier;
                ApplyToClass = applyToClass;
                IsNullableEnabled = isNullableEnabled;
            }

            public string ActualTypeFullName { get;  }

            public string Identifier { get; }

            public ClassDeclarationSyntax ApplyToClass { get; }

            public bool IsNullableEnabled { get; }
        }

        private sealed class Output
        {
            public Output(BaseListSyntax baseListTypes, IImmutableList<MemberDeclarationSyntax> members)
            {
                BaseListTypes = baseListTypes;
                Members = members;
            }

            public Output(IImmutableList<MemberDeclarationSyntax> members)
                : this((SimpleBaseTypeSyntax) null!, members)
            {
                // nothing to do here
            }

            public Output(SimpleBaseTypeSyntax? baseType, IImmutableList<MemberDeclarationSyntax> members)
                : this(baseType is null ? null! : BaseList(SeparatedList<BaseTypeSyntax>(new [] { baseType })), members)
            {
                // nothing to do here
            }

            public BaseListSyntax BaseListTypes { get; }

            public IImmutableList<MemberDeclarationSyntax> Members { get; }
        }

        private delegate Output Generate(Input input);

        public SemanticTypeGenerator(AttributeData attributeData)
        {
            if (attributeData is null) throw new ArgumentNullException(nameof(attributeData));

            _actualType = attributeData.ConstructorArguments[0];
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

            var nullableContext = context.SemanticModel.GetNullableContext(applyToClass.SpanStart);
            var isNullableEnabled = nullableContext == NullableContext.Enabled;

            var generators = new Generate[]
            {
                GenerateConstructorAndField,
                GenerateComparable,
                GenerateEquality,
                GenerateExplicitOperator,
                GenerateToString
            };
            var accessibility = classSymbol.DeclaredAccessibility;
            var baseTypes = new List<BaseTypeSyntax>();
            var members = new List<MemberDeclarationSyntax>();
            var input = new Input(_actualType.Value!.ToString(), idName, applyToClass, isNullableEnabled);
            foreach (var generator in generators)
            {
                var output = generator(input);
                if (output.BaseListTypes != null)
                {
                    baseTypes.AddRange(output.BaseListTypes.Types);
                }

                members.AddRange(output.Members);
            }

            members.AddRange(AspNetCoreModelBinding.Generate(_actualType, context, input.Identifier));
            members.AddRange(JsonNetConverter.Generate(_actualType, context, input.Identifier));
            members.AddRange(SystemTextJsonConverter.Generate(_actualType, context, input.Identifier));

            var leading = isNullableEnabled ? TriviaList(Trivia(NullableDirectiveTrivia(Token(SyntaxKind.EnableKeyword), true))) : SyntaxTriviaList.Empty;
            var trailing = isNullableEnabled ? TriviaList(Trivia(NullableDirectiveTrivia(Token(SyntaxKind.RestoreKeyword), true))) : SyntaxTriviaList.Empty;

            var result = SingletonList<MemberDeclarationSyntax>(
                ClassDeclaration(applyToClass.Identifier.ValueText)
                    .WithBaseList(BaseList(SeparatedList(baseTypes)))
                    .WithModifiers(TokenList( ParseToken(SyntaxFacts.GetText(accessibility)), Token(SyntaxKind.PartialKeyword)))
                    .WithLeadingTrivia(leading)
                    .WithMembers(List(members))
                    .WithTrailingTrivia(trailing));
            var wrappedMembers = result.WrapWithAncestors(context.ProcessingNode);

            return Task.FromResult(new RichGenerationResult
            {
                Members = new SyntaxList<MemberDeclarationSyntax>(wrappedMembers)
            });
        }
    }
}
