using System;
using System.Diagnostics;
using CodeGeneration.Roslyn;

namespace Obviously.Exceptional.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    [CodeGenerationAttribute(typeof(ExceptionGenerator))]
    [Conditional("CodeGeneration")]
    public sealed class ExceptionalAttribute : Attribute
    {
    }
}
