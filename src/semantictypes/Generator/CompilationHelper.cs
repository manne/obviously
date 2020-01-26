using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Obviously.SemanticTypes.Generator
{
    internal static class CompilationHelper
    {
        public static bool HasExternalReference(this Compilation compilation, string referenceName) =>
             compilation.ExternalReferences
                .Select(er => er.Display)
                .Select(Path.GetFileNameWithoutExtension)
                .FirstOrDefault(assemblyName => assemblyName == referenceName) != null;
    }
}
