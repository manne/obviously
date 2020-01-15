using System.Diagnostics.CodeAnalysis;

namespace Obviously.System.Text.Json.Tests
{
#pragma warning disable IDE0060 // Remove unused parameter
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public static class TestingNameMatchingTypes
    {
        public sealed class WithOneMatchingParameter
        {
            public WithOneMatchingParameter(string foo)
            {
                Foo = foo;
            }

            public string Foo { get; }
        }

        public sealed class WithOneNonMatchingParameter
        {
            public WithOneNonMatchingParameter(string bar)
            {
                Foo = bar;
            }

            public string Foo { get; }
        }
    }
}
