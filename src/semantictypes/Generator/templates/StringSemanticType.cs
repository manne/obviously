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

    public class Int32EqualitySemanticType : IEquatable<Int32EqualitySemanticType>
    {
        public bool Equals(Int32EqualitySemanticType other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Int32EqualitySemanticType)obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(Int32EqualitySemanticType left, Int32EqualitySemanticType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Int32EqualitySemanticType left, Int32EqualitySemanticType right)
        {
            return !Equals(left, right);
        }

        private readonly Guid _value;
    }
}
