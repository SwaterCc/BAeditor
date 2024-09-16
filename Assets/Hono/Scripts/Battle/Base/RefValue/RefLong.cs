using System;

namespace Hono.Scripts.Battle.RefValue
{
    [Serializable]
    public class RefLong
    {
        public long Value;

        public RefLong(long initialValue = 0L)
        {
            Value = initialValue;
        }

        // 隐式转换到 long
        public static implicit operator long(RefLong refLong)
        {
            return refLong.Value;
        }

        // 隐式转换从 long
        public static implicit operator RefLong(long value)
        {
            return new RefLong(value);
        }

        // 自增操作符重载
        public static RefLong operator ++(RefLong refLong)
        {
            refLong.Value++;
            return refLong;
        }

        // 自减操作符重载
        public static RefLong operator --(RefLong refLong)
        {
            refLong.Value--;
            return refLong;
        }

        // 加法操作符重载
        public static RefLong operator +(RefLong a, RefLong b)
        {
            return new RefLong(a.Value + b.Value);
        }

        // 减法操作符重载
        public static RefLong operator -(RefLong a, RefLong b)
        {
            return new RefLong(a.Value - b.Value);
        }

        // 乘法操作符重载
        public static RefLong operator *(RefLong a, RefLong b)
        {
            return new RefLong(a.Value * b.Value);
        }

        // 除法操作符重载
        public static RefLong operator /(RefLong a, RefLong b)
        {
            if (b.Value == 0) throw new DivideByZeroException();
            return new RefLong(a.Value / b.Value);
        }

        // 大于操作符重载
        public static bool operator >(RefLong a, RefLong b)
        {
            return a.Value > b.Value;
        }

        // 小于操作符重载
        public static bool operator <(RefLong a, RefLong b)
        {
            return a.Value < b.Value;
        }

        // 大于等于操作符重载
        public static bool operator >=(RefLong a, RefLong b)
        {
            return a.Value >= b.Value;
        }

        // 小于等于操作符重载
        public static bool operator <=(RefLong a, RefLong b)
        {
            return a.Value <= b.Value;
        }
    }
}