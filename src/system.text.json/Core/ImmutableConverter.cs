using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Obviously.System.Text.Json
{
    public class ImmutableConverter : JsonConverter<object>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            bool result;
            var constructors = typeToConvert.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Length != 1)
            {
                result = false;
            }
            else
            {
                var constructor = constructors[0];
                var parameters = constructor.GetParameters();
                var hasParameters = parameters.Length > 0;
                if (hasParameters)
                {
                    var properties = typeToConvert.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    result = true;
                    foreach (var parameter in parameters)
                    {
                        var hasMatchingProperty = properties.Any(p =>
                            NameOfPropertyAndParameter.Matches(p.Name, parameter.Name));
                        if (!hasMatchingProperty)
                        {
                            result = false;
                            break;
                        }
                    }
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    internal class NameOfPropertyAndParameter 
    {
        public static bool Matches(string propertyName, string parameterName)
        {
            if (propertyName is null && parameterName is null)
            {
                return true;
            }

            if (propertyName is null || parameterName is null)
            {
                return false;
            }

            var xRight = propertyName.AsSpan(1);
            var yRight = parameterName.AsSpan(1);
            return char.ToLowerInvariant(propertyName[0]).CompareTo(parameterName[0]) == 0 && xRight.Equals(yRight, StringComparison.Ordinal);
        }
    }
}