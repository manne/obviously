using System;
using System.Diagnostics.CodeAnalysis;

namespace Obviously.SemanticTypes.Generator.Templates.AspNetCore.ModelBinding
{
    [SuppressMessage("ReSharper", "RedundantNameQualifier")]
#pragma warning disable IDE0001
    public sealed class ManualGuidSemanticTypeModelBinderProvider : global::Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinderProvider
    {
        public global::Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder GetBinder(global::Microsoft.AspNetCore.Mvc.ModelBinding.ModelBinderProviderContext context)
        {
            if (context is null) throw new global::System.ArgumentNullException(nameof(context));
            var metadataForType = context.MetadataProvider.GetMetadataForType(typeof(Guid));
            var specificBinder = context.CreateBinder(metadataForType);
            return context.Metadata.ModelType == typeof(ManualGuidSemanticType) ? new ManualGuidSemanticTypeModelBinder(metadataForType, specificBinder) : null;
        }
    }
}
