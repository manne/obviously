using System;
using System.Collections.Generic;
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

            var constructors = new List<ConstructorDeclarationSyntax>(3)
            {
                GetConstructorDefault(idName),
                GetConstructorWithMessage(idName),
                GetConstructorWithMessageAndInnerException(idName)
            };

            var baseList = applyToClass.DescendantNodes().OfType<BaseListSyntax>().FirstOrDefault();
            var classWithoutBaseList = applyToClass.RemoveNode(baseList, SyntaxRemoveOptions.KeepNoTrivia);


            var a = classWithoutBaseList
                .WithAttributeLists(new SyntaxList<AttributeListSyntax>())
                .WithMembers(new SyntaxList<MemberDeclarationSyntax>(constructors));
            return Task.FromResult(new SyntaxList<MemberDeclarationSyntax>(a));
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
    }
}
