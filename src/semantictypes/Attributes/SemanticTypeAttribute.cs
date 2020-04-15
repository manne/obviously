using System;
using System.Diagnostics;
using CodeGeneration.Roslyn;

namespace Obviously.SemanticTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    [CodeGenerationAttribute("Obviously.SemanticTypes.Generator.SemanticTypeGenerator, Obviously.SemanticTypes.Generator")]
    [Conditional("CodeGeneration")]
    public sealed class SemanticTypeAttribute : Attribute
    {
        public SemanticTypeAttribute(Type actualType)
        {
            ActualType = actualType;
        }

        /// <summary>
        /// Gets or sets the actual semantic type.
        /// </summary>
        public Type ActualType { get; set; }

        /// <summary>
        /// Indicates whether the <see cref="ActualType"/> is <c>nullable</c>.
        /// </summary>
        /// <remarks>
        /// The nullable context should be <c>enable</c>d.
        /// </remarks>
        /// <remarks>
        /// This property is needed, because this <c>typeof(string?)</c> is not valid.
        /// </remarks>
        public bool IsNullableType { get; set; }
    }
}
