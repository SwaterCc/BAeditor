using System;

namespace Hono.Scripts.Battle.RefValue
{
    public abstract class RefValue<T> where T : struct, IComparable<T>, IEquatable<T>
    {
        public T Value { get; set; }

        protected RefValue(T initialValue)
        {
            Value = initialValue;
        }

        // 操作符重载
        public static bool operator ==(RefValue<T> a, RefValue<T> b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Value.Equals(b.Value);
        }

        public static bool operator !=(RefValue<T> a, RefValue<T> b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is RefValue<T> refValue)
            {
                return Value.Equals(refValue.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}