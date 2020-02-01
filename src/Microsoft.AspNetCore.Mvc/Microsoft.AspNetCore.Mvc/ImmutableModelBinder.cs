using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using PnCpComparer = Obviously.Microsoft.AspNetCore.Mvc.ModelBinding.Internals.PropertyNameConstructorParameterNameEqualityComparer;

// ReSharper disable once CheckNamespace
namespace Obviously.Microsoft.AspNetCore.Mvc.ModelBinding
{
    public class ImmutableModelBinder : IModelBinder
    {
        private readonly ImmutableDictionary<ModelMetadata, IModelBinder> _modelBinders;

        public ImmutableModelBinder(ImmutableDictionary<ModelMetadata, IModelBinder> modelBinders)
        {
            _modelBinders = modelBinders ?? throw new ArgumentNullException(nameof(modelBinders));
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var constructors = bindingContext.ModelType.GetConstructors();
            if (constructors.Length != 1)
            {
                throw new InvalidOperationException($"The type '{bindingContext.ModelType}' must have exactly one constructor, but found {constructors.Length}");
            }

            var constructor = constructors[0];
            var parameters = constructor.GetParameters();
            if (parameters.Length == 0)
            {
                throw new InvalidOperationException("the constructor must have at least one parameter, but has 0");
            }

            var propertyCount = bindingContext.ModelType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Length;

            if (parameters.Length != propertyCount)
            {
                throw new InvalidOperationException(FormattableString.Invariant($"The count of the properties ({propertyCount}) and parameters of the constructor ({parameters.Length}) must be the same"));
            }

            var allValuesBuilt = true;
            var modelValues = new Dictionary<string, object>();
            foreach (var modelMetadataProperty in bindingContext.ModelMetadata.Properties)
            {
                var fieldName = modelMetadataProperty.BinderModelName ?? modelMetadataProperty.PropertyName;
                var modelName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, fieldName);
                using (bindingContext.EnterNestedScope(modelMetadataProperty, fieldName, modelName, null))
                {
                    var modelBinder = _modelBinders[bindingContext.ModelMetadata];
                    await modelBinder.BindModelAsync(bindingContext);

                    if (bindingContext.Result.IsModelSet)
                    {
                        // the property name is used, because the binder model name could be a different, then the parameter matching logic would not work
                        modelValues.Add(modelMetadataProperty.PropertyName, bindingContext.Result.Model);
                    }
                    else
                    {
                        // if a value for a value type is not present, the creation of an instance would be successful, but the instance would be inconsistent.
                        // Example:
                        // public class Foo { public Foo(int bar){...}...}
                        // invoking the constructor via reflection with null for bar, works. The value of the property is 0.
                        allValuesBuilt = false;
                        break;
                    }
                }
            }

            if (!allValuesBuilt)
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }
            else
            {
                var parameterValues = new object[parameters.Length];
                for (var index = 0; index < parameters.Length; index++)
                {
                    var parameterInfo = parameters[index];
                    var (key, value) = modelValues.FirstOrDefault(prop =>
                        PnCpComparer.Instance.Equals(prop.Key, parameterInfo.Name!));

                    Debug.Assert(key != null, "key != null");

                    parameterValues[index] = value;
                }

                var model = constructor.Invoke(parameterValues);
                bindingContext.Result = ModelBindingResult.Success(model);
            }
        }
    }
}
