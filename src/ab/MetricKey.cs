using System;

namespace ab
{
    public struct MetricKey : IComparable<MetricKey>
    {
        public Type Class { get; private set; }
        public string Name { get; private set; }

        public MetricKey(Type @class, string name) : this()
        {
            Class = @class;
            Name = name;
        }

        public bool Equals(MetricKey other)
        {
            return Equals(other.Name, Name) && other.Class == Class;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is MetricKey && Equals((MetricKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Class != null ? Class.GetHashCode() : 0);
            }
        }

        public static bool operator ==(MetricKey left, MetricKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MetricKey left, MetricKey right)
        {
            return !left.Equals(right);
        }

        public int CompareTo(MetricKey other)
        {
            return string.Compare(string.Concat(Class, ".", Name), string.Concat(other.Class, ".", other.Name), StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return string.Concat(Class.Name, ".", Name);
        }
    }
}