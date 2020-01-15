using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Obviously.System.Text.Json.Tests
{
#pragma warning disable IDE0060 // Remove unused parameter
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public static class TestingTypeMatchingTypes
    {
        public sealed class Basic
        {
            public Basic(string foo)
            {
                Foo = foo;
            }

            public string Foo { get; }
        }

        public sealed class BasicWithJsonPropertyName
        {
            public BasicWithJsonPropertyName(string foo)
            {
                Foo = foo;
            }

            [JsonPropertyName("lol")]
            public string Foo { get; }
        }
    }
}
