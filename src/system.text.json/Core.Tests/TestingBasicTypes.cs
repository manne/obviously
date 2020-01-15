using System.Diagnostics.CodeAnalysis;

namespace Obviously.System.Text.Json.Tests
{
#pragma warning disable IDE0060 // Remove unused parameter
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public static class TestingBasicTypes
    {
        public sealed class TypeWithConstructorAndNoParameter
        {
            // nothing to do here
        }

        public sealed class TypeWithTwoConstructors
        {
            public TypeWithTwoConstructors(string dummy)
            {
                // nothing to do here
            }

            public TypeWithTwoConstructors(int dummy)
            {
                // nothing to do here
            }
        }

        public sealed class TypeWithOneInt32Property
        {
            public TypeWithOneInt32Property(int dummy)
            {
                Dummy = dummy;
            }

            public int Dummy { get; }
        }
    }
}
