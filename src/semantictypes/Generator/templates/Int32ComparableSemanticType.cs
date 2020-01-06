using System;

namespace Obviously.SemanticTypes.Generator.templates
{
    public class Int32ComparableSemanticType : IComparable<Int32ComparableSemanticType>, IComparable
    {
        public int CompareTo(Int32ComparableSemanticType? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;
            return _value.CompareTo(other._value);
        }

        public int CompareTo(object? obj)
        {
            if (obj is null) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            return obj is Int32ComparableSemanticType other ? CompareTo(other) : throw new ArgumentException("Object must be of type ZZZZZZ");
        }

        public static bool operator <(Int32ComparableSemanticType? left, Int32ComparableSemanticType? right)
        {
            return global::System.Collections.Generic.Comparer<Int32ComparableSemanticType>.Default.Compare(left!, right!) < 0;
        }

        public static bool operator >(Int32ComparableSemanticType? left, Int32ComparableSemanticType? right)
        {
            return global::System.Collections.Generic.Comparer<Int32ComparableSemanticType>.Default.Compare(left!, right!) > 0;
        }

        public static bool operator <=(Int32ComparableSemanticType? left, Int32ComparableSemanticType? right)
        {
            return global::System.Collections.Generic.Comparer<Int32ComparableSemanticType>.Default.Compare(left!, right!) <= 0;
        }

        public static bool operator >=(Int32ComparableSemanticType? left, Int32ComparableSemanticType? right)
        {
            return global::System.Collections.Generic.Comparer<Int32ComparableSemanticType>.Default.Compare(left!, right!) >= 0;
        }

        private readonly int _value = 123;
    }
}
