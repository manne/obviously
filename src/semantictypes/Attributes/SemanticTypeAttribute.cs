using System;
using System.Diagnostics;
using CodeGeneration.Roslyn;

namespace Obviously.SemanticTypes.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    [CodeGenerationAttribute("Obviously.SemanticTypes.Generator.SemanticTypeGenerator, Obviously.SemanticTypes.Generator")]
    [Conditional("CodeGeneration")]
    public sealed class SemanticTypeAttribute : Attribute
    {
        public SemanticTypeAttribute(Type actualType)
        {
            ActualType = actualType;
        }

        public Type ActualType { get; set; }
    }
}
