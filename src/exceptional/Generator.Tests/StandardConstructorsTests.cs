using System;
using Obviously.Exceptional.Generator;
using Xunit;

namespace Generator.Tests
{
    [Exceptional]
    public partial class RudimentaryException : Exception
    {

    }

    public class StandardConstructorsTests
    {
        [Fact]
        public void ConstructorWithNoParameter_ShouldExist()
        {
            _ = new RudimentaryException();
        }

        [Fact]
        public void ConstructorWithMessageParameter_ShouldExist()
        {
            _ = new RudimentaryException("hello world!");
        }

        [Fact]
        public void ConstructorWithMessageAndInnerExceptionParameters_ShouldExist()
        {
            _ = new RudimentaryException("hello world!", new Exception());
        }
    }
}
