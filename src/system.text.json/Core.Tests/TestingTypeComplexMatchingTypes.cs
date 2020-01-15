using System;
using System.Collections.Immutable;
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

        public class WithTwoProperties
        {
            public WithTwoProperties(string cool, IImmutableList<string> bar)
            {
                Bar = bar;
                Cool = cool;
            }

            public IImmutableList<string> Bar { get; }

            public string Cool { get; }
        }

        public class WithTwoProperties2
        {
            public WithTwoProperties2(string cool, Guid bar)
            {
                Bar = bar;
                Cool = cool;
            }

            public Guid Bar { get; }

            public string Cool { get; }
        }

        public class WithOneGenericProperties<T>
        {
            public WithOneGenericProperties(T cool)
            {
                Cool = cool;
            }

            public T Cool { get; }
        }
    }
}
