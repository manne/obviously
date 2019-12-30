using System;
using System.Collections.Generic;
using System.Text;

namespace Obviously.SemanticTypes.Generator.templates
{
    public class StringSemanticType
    {
        private readonly string _value;
        public StringSemanticType(string value)
        {
            _value = value;
        }
    }

    public class GuidSemanticType : IComparable<GuidSemanticType>
    {
        public int CompareTo(GuidSemanticType other)
        {
            return _value.CompareTo(other._value);
        }

        private readonly Guid _value;
    }
}
