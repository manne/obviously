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

            var hasAspNetCoreMvc = context.SemanticModel.Compilation.HasType("Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinderProvider");
            if (!hasAspNetCoreMvc)
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
