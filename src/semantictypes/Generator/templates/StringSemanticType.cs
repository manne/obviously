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

        public override bool Equals(object other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (!(other is Int32EqualitySemanticType other2))
            {
                return false;
            }

            return _value == other2._value;
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

        private readonly Int32 _value;
    }

    public class Int32EqualitySemanticType 
    {
        public Int32EqualitySemanticType(Int32 value)
        {
            if(!IsValid(value)) throw new ArgumentException("The parameter is invalid", "value");

            _value = value;
        }
        private readonly Int32 _value;
    }

    public class Int32ComparableSemanticType : IComparable<Int32ComparableSemanticType>
    {
        public int CompareTo(Int32ComparableSemanticType other)
        {
            return other is null ? 1 : _value.CompareTo(other._value);
        }
        public static bool operator <(Int32ComparableSemanticType left, Int32ComparableSemanticType right)
        {
            return left is null ? !(right is null) : left.CompareTo(right) < 0;
        }

        public static bool operator <=(Int32ComparableSemanticType left, Int32ComparableSemanticType right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        public static bool operator >(Int32ComparableSemanticType left, Int32ComparableSemanticType right)
        {
            return !(left is null) && left.CompareTo(right) > 0;
        }

        public static bool operator >=(Int32ComparableSemanticType left, Int32ComparableSemanticType right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }

        private readonly int _value = 123;
    }
}
