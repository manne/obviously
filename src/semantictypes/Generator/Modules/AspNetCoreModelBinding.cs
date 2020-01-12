using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

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

            var hasAspNetCoreMvcReference = context.SemanticModel.Compilation.ExternalReferences
                .Select(er => er.Display)
                .Select(Path.GetFileNameWithoutExtension)
                .FirstOrDefault(assemblyName => assemblyName == "Microsoft.AspNetCore.Mvc");
            if (hasAspNetCoreMvcReference is null)
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
