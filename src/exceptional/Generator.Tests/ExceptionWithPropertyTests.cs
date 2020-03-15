using System;
using Obviously.Exceptional.Generator;
using Xunit;

namespace Generator.Tests
{
    [Exceptional]
    public sealed partial class ExceptionWithPropertyException : Exception
    {
        public string Foo { get; }
        public int Bar { get; }
    } 

    public class ExceptionWithPropertyTests
    {
        [Fact]
        public void ExceptionTypesWithCustomProperties_ShouldBeUsable()
        {
            _ = new ExceptionWithPropertyException();
        }
    }
}
