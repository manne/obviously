using System;
using System.Diagnostics.CodeAnalysis;

namespace Generator.Templates.AspNetCore.ModelBinding
{
    [SuppressMessage("ReSharper", "RedundantNameQualifier")]
#pragma warning disable IDE0001
    public sealed class AwesomeSemanticTypeModelBinderProvider : global::Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinderProvider
    {
        public global::Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder GetBinder(global::Microsoft.AspNetCore.Mvc.ModelBinding.ModelBinderProviderContext context)
        {
            if (context is null) throw new global::System.ArgumentNullException(nameof(context));
            return context.Metadata.ModelType == typeof(AwesomeInt32SemanticType) ? new AwesomeSemanticTypeModelBinder() : null;
        }
    }
}
