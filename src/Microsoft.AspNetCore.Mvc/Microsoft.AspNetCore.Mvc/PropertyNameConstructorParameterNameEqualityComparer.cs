using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Obviously.Microsoft.AspNetCore.Mvc.ModelBinding.Internals
{
    internal class PropertyNameConstructorParameterNameEqualityComparer : IEqualityComparer<string>
    {
        public static PropertyNameConstructorParameterNameEqualityComparer Instance { get; } = new PropertyNameConstructorParameterNameEqualityComparer();

        private PropertyNameConstructorParameterNameEqualityComparer()
        {
            // nothing to do here
        }

        public bool Equals(string x, string y)
        {
            if (x is null && y is null)
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            var xRight = x.AsSpan(1);
            var yRight = y.AsSpan(1);
            return char.ToLowerInvariant(x[0]) == y[0] && xRight.Equals(yRight, StringComparison.Ordinal);
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}