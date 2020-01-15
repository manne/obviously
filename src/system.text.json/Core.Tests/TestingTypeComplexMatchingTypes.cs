using System.Diagnostics.CodeAnalysis;

namespace Obviously.System.Text.Json.Tests
{
#pragma warning disable IDE0060 // Remove unused parameter
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public static class TestingTypeComplexMatchingTypes
    {
        public sealed class Basic
        {
            public Basic(string foo, string xyz)
            {
                Foo = foo;
                Xyz = xyz;
            }

            public string Foo { get; }

            public string Xyz { get; }
        }
    }
}
