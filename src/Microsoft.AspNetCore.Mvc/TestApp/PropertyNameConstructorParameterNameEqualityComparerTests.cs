using FluentAssertions;
using Obviously.Microsoft.AspNetCore.Mvc.ModelBinding.Internals;
using Xunit;

namespace TestApp
{
    public class PropertyNameConstructorParameterNameEqualityComparerTests
    {
        private static readonly PropertyNameConstructorParameterNameEqualityComparer Instance = PropertyNameConstructorParameterNameEqualityComparer.Instance;

        [Theory]
        [InlineData("Name", "name", true)]
        [InlineData("PageNumber", "pageNumber", true)]
        public void Check(string propertyName, string parameterName, bool expectedResult)
        {
            Instance.Equals(propertyName, parameterName).Should().Be(expectedResult);
        }
    }
}
