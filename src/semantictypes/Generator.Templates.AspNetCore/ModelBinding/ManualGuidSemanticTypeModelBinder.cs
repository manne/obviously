using System;
using System.Diagnostics.CodeAnalysis;

namespace Obviously.SemanticTypes.Generator.Templates.AspNetCore.ModelBinding
{
    // this class serves as the blueprint for the actual generator implementation
    [SuppressMessage("ReSharper", "RedundantNameQualifier")]
#pragma warning disable IDE0001
#pragma warning disable IDE0002
    public sealed class ManualGuidSemanticTypeModelBinder : global::Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder
    {
        private readonly global::Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata _metadataForType;
        private readonly global::Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder _specificBinder;

        public ManualGuidSemanticTypeModelBinder(global::Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata metadataForType, global::Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder specificBinder)
        {
            _metadataForType = metadataForType;
            _specificBinder = specificBinder;
        }

        public async global::System.Threading.Tasks.Task BindModelAsync(global::Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var actualValue = value.FirstValue;
            Guid modelBindingResult;
            using (bindingContext.EnterNestedScope(_metadataForType, "Value", bindingContext.ModelName, actualValue))
            {
                var newBindingContext = global::Microsoft.AspNetCore.Mvc.ModelBinding.DefaultModelBindingContext.CreateBindingContext(
                    bindingContext.ActionContext,
                    bindingContext.ValueProvider,
                    _metadataForType,
                    null,
                    bindingContext.ModelName);
                await _specificBinder.BindModelAsync(newBindingContext).ConfigureAwait(false);
                modelBindingResult = (Guid)newBindingContext.Result.Model;
            }

            var modelInstance = new ManualGuidSemanticType(modelBindingResult);
            bindingContext.Result = global::Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingResult.Success(modelInstance);
        }
    }
}
