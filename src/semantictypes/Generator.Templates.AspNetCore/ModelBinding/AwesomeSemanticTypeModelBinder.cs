namespace Generator.Templates.AspNetCore.ModelBinding
{
    public sealed class AwesomeSemanticTypeModelBinder : global::Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder
    {
        public global::System.Threading.Tasks.Task BindModelAsync(global::Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelMetadata.Name);
            var actualValue = value.FirstValue;
            var concreteValue = (int) global::System.Convert.ChangeType(actualValue, typeof(int));
            var modelInstance = new AwesomeInt32SemanticType(concreteValue);
            bindingContext.Result = global::Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingResult.Success(modelInstance);
            return global::System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
