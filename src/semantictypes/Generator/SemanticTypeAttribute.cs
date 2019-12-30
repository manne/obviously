using System;
using System.Diagnostics;
using CodeGeneration.Roslyn;

namespace Obviously.SemanticTypes.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    [CodeGenerationAttribute(typeof(SemanticTypeGenerator))]
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
