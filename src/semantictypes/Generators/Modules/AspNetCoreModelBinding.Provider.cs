﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Obviously.SemanticTypes.Generators.Modules
{
    public partial class AspNetCoreModelBinding
    {
        private static ClassDeclarationSyntax ConstructProvider(string identifierName)
        {
            var className = PrefixForSubClasses + "ModelBinderProvider";
            var binderClassName = PrefixForSubClasses + "ModelBinder";
            var binderProvider = ClassDeclaration(className)
                .WithModifiers(
	                TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.SealedKeyword)))
                .WithBaseList(
	                BaseList(
		                SingletonSeparatedList<BaseTypeSyntax>(
			                SimpleBaseType(
				                QualifiedName(
					                QualifiedName(
						                QualifiedName(
							                QualifiedName(
								                AliasQualifiedName(
									                IdentifierName(
										                Token(SyntaxKind.GlobalKeyword)),
									                IdentifierName("Microsoft")),
								                IdentifierName("AspNetCore")),
							                IdentifierName("Mvc")),
						                IdentifierName("ModelBinding")),
					                IdentifierName("IModelBinderProvider"))))))
                .WithMembers(
	                SingletonList<MemberDeclarationSyntax>(
		                MethodDeclaration(
			                QualifiedName(
				                QualifiedName(
					                QualifiedName(
						                QualifiedName(
							                AliasQualifiedName(
								                IdentifierName(
									                Token(SyntaxKind.GlobalKeyword)),
								                IdentifierName("Microsoft")),
							                IdentifierName("AspNetCore")),
						                IdentifierName("Mvc")),
					                IdentifierName("ModelBinding")),
				                IdentifierName("IModelBinder")),
			                Identifier("GetBinder"))
		                .WithModifiers(
			                TokenList(
				                Token(SyntaxKind.PublicKeyword)))
		                .WithParameterList(
			                ParameterList(
				                SingletonSeparatedList(
					                Parameter(
						                Identifier("context"))
					                .WithType(
						                QualifiedName(
							                QualifiedName(
								                QualifiedName(
									                QualifiedName(
										                AliasQualifiedName(
											                IdentifierName(
												                Token(SyntaxKind.GlobalKeyword)),
											                IdentifierName("Microsoft")),
										                IdentifierName("AspNetCore")),
									                IdentifierName("Mvc")),
								                IdentifierName("ModelBinding")),
							                IdentifierName("ModelBinderProviderContext"))))))
		                .WithBody(
			                Block(
				                IfStatement(
					                IsPatternExpression(
						                IdentifierName("context"),
						                ConstantPattern(
							                LiteralExpression(
								                SyntaxKind.NullLiteralExpression))),
					                ThrowStatement(
						                ObjectCreationExpression(
							                QualifiedName(
								                AliasQualifiedName(
									                IdentifierName(
										                Token(SyntaxKind.GlobalKeyword)),
									                IdentifierName("System")),
								                IdentifierName("ArgumentNullException")))
						                .WithArgumentList(
							                ArgumentList(
								                SingletonSeparatedList(
									                Argument(
										                InvocationExpression(
											                IdentifierName("nameof"))
										                .WithArgumentList(
											                ArgumentList(
												                SingletonSeparatedList(
													                Argument(
														                IdentifierName("context"))))))))))),
				                LocalDeclarationStatement(
					                VariableDeclaration(
						                IdentifierName("var"))
					                .WithVariables(
						                SingletonSeparatedList(
							                VariableDeclarator(
								                Identifier("metadataForType"))
							                .WithInitializer(
								                EqualsValueClause(
									                InvocationExpression(
										                MemberAccessExpression(
											                SyntaxKind.SimpleMemberAccessExpression,
											                MemberAccessExpression(
												                SyntaxKind.SimpleMemberAccessExpression,
												                IdentifierName("context"),
												                IdentifierName("MetadataProvider")),
											                IdentifierName("GetMetadataForType")))
									                .WithArgumentList(
										                ArgumentList(
											                SingletonSeparatedList(
												                Argument(
													                TypeOfExpression(
														                IdentifierName("Guid"))))))))))),
				                LocalDeclarationStatement(
					                VariableDeclaration(
						                IdentifierName("var"))
					                .WithVariables(
						                SingletonSeparatedList(
							                VariableDeclarator(
								                Identifier("specificBinder"))
							                .WithInitializer(
								                EqualsValueClause(
									                InvocationExpression(
										                MemberAccessExpression(
											                SyntaxKind.SimpleMemberAccessExpression,
											                IdentifierName("context"),
											                IdentifierName("CreateBinder")))
									                .WithArgumentList(
										                ArgumentList(
											                SingletonSeparatedList(
												                Argument(
													                IdentifierName("metadataForType")))))))))),
				                ReturnStatement(
					                ConditionalExpression(
						                BinaryExpression(
							                SyntaxKind.EqualsExpression,
							                MemberAccessExpression(
								                SyntaxKind.SimpleMemberAccessExpression,
								                MemberAccessExpression(
									                SyntaxKind.SimpleMemberAccessExpression,
									                IdentifierName("context"),
									                IdentifierName("Metadata")),
								                IdentifierName("ModelType")),
							                TypeOfExpression(
								                IdentifierName(identifierName))),
						                ObjectCreationExpression(
							                IdentifierName(binderClassName))
						                .WithArgumentList(
							                ArgumentList(
								                SeparatedList<ArgumentSyntax>(
									                new SyntaxNodeOrToken[]{
										                Argument(
											                IdentifierName("metadataForType")),
										                Token(SyntaxKind.CommaToken),
										                Argument(
											                IdentifierName("specificBinder"))}))),
						                LiteralExpression(
							                SyntaxKind.NullLiteralExpression)))))));

            return binderProvider;

        }
    }
}
