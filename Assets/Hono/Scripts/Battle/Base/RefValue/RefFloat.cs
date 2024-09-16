using System;

namespace Hono.Scripts.Battle.RefValue
{
    [Serializable]
    public class RefFloat
    {
        public float Value;

        public RefFloat(float initialValue = 0f)
        {
            Value = initialValue;
        }

        // 显式转换到 RefInt 注意会产生新的对象
        public static explicit operator RefInt(RefFloat refFloat)
        {
            return new RefInt((int)refFloat.Value);
        }

        // 隐式转换到 float
        public static implicit operator float(RefFloat refFloat)
        {
            return refFloat.Value;
        }

        // 隐式转换从 float
        public static implicit operator RefFloat(float value)
        {
            return new RefFloat(value);
        }

        // 自增操作符重载
        public static RefFloat operator ++(RefFloat refFloat)
        {
            refFloat.Value++;
            return refFloat;
        }

        // 自减操作符重载
        public static RefFloat operator --(RefFloat refFloat)
        {
            refFloat.Value--;
            return refFloat;
        }

        // 加法操作符重载
        public static RefFloat operator +(RefFloat a, RefFloat b)
        {
            return new RefFloat(a.Value + b.Value);
        }

        // 减法操作符重载
        public static RefFloat operator -(RefFloat a, RefFloat b)
        {
            return new RefFloat(a.Value - b.Value);
        }

        // 乘法操作符重载
        public static RefFloat operator *(RefFloat a, RefFloat b)
        {
            return new RefFloat(a.Value * b.Value);
        }

        // 除法操作符重载
        public static RefFloat operator /(RefFloat a, RefFloat b)
        {
            if (b.Value == 0) throw new DivideByZeroException();
            return new RefFloat(a.Value / b.Value);
        }

        // 大于操作符重载
        public static bool operator >(RefFloat a, RefFloat b)
        {
            return a.Value > b.Value;
        }

        // 小于操作符重载
        public static bool operator <(RefFloat a, RefFloat b)
        {
            return a.Value < b.Value;
        }

        // 大于等于操作符重载
        public static bool operator >=(RefFloat a, RefFloat b)
        {
            return a.Value >= b.Value;
        }

        // 小于等于操作符重载
        public static bool operator <=(RefFloat a, RefFloat b)
        {
            return a.Value <= b.Value;
        }
    }
}