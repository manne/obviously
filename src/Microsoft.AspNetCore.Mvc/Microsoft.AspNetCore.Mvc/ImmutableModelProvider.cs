using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// ReSharper disable once CheckNamespace
namespace Obviously.Microsoft.AspNetCore.Mvc.ModelBinding
{
    public class ImmutableModelProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var modelType = context.Metadata.ModelType;
            var constructorInfos = modelType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructorInfos.Length != 1)
            {
                return null!;
            }

            if (constructorInfos[0].GetParameters().Length == 0)
            {
                return null!;
            }

            var propertyBinders = new Dictionary<ModelMetadata, IModelBinder>();
            foreach (var property in context.Metadata.Properties)
            {
                propertyBinders.Add(property, context.CreateBinder(property));
            }

            return new ImmutableModelBinder(propertyBinders.ToImmutableDictionary());
        }
    }
}