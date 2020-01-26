using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Obviously.SemanticTypes.Generator.Modules
{
    public static partial class AspNetCoreModelBinding
    {
        private const string PrefixForSubClasses = "AspNetCoreMvc";

        public static ImmutableArray<ClassDeclarationSyntax> Generate(TypedConstant actualType, TransformationContext context, string identifierName)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (identifierName is null) throw new ArgumentNullException(nameof(identifierName));

            var hasNotAspNetCoreMvcReference = !context.SemanticModel.Compilation.HasExternalReference("Microsoft.AspNetCore.Mvc");
            if (hasNotAspNetCoreMvcReference)
            {
                return ImmutableArray<ClassDeclarationSyntax>.Empty;
            }

            var list = new List<ClassDeclarationSyntax>
            {
                ConstructBinder(actualType, identifierName),
                ConstructProvider(identifierName)
            };
            return ImmutableArray.CreateRange(list);
        }
    }
}
