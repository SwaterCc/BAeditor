using System;

namespace Hono.Scripts.Battle.RefValue
{
    public class RefInt : RefValue<int>
{
    public RefInt(int initialValue = 0) : base(initialValue) { }

    // 隐式转换到 RefFloat
    public static implicit operator RefFloat(RefInt refInt)
    {
        return new RefFloat(refInt.Value);
    }

    // 隐式转换到 int
    public static implicit operator int(RefInt refInt)
    {
        return refInt.Value;
    }

    // 隐式转换从 int
    public static implicit operator RefInt(int value)
    {
        return new RefInt(value);
    }

    // 自增操作符重载
    public static RefInt operator ++(RefInt refInt)
    {
        refInt.Value++;
        return refInt;
    }

    // 自减操作符重载
    public static RefInt operator --(RefInt refInt)
    {
        refInt.Value--;
        return refInt;
    }

    // 加法操作符重载
    public static RefInt operator +(RefInt a, RefInt b)
    {
        return new RefInt(a.Value + b.Value);
    }

    // 减法操作符重载
    public static RefInt operator -(RefInt a, RefInt b)
    {
        return new RefInt(a.Value - b.Value);
    }

    // 乘法操作符重载
    public static RefInt operator *(RefInt a, RefInt b)
    {
        return new RefInt(a.Value * b.Value);
    }

    // 除法操作符重载
    public static RefInt operator /(RefInt a, RefInt b)
    {
        if (b.Value == 0) throw new DivideByZeroException();
        return new RefInt(a.Value / b.Value);
    }

    // 大于操作符重载
    public static bool operator >(RefInt a, RefInt b)
    {
        return a.Value > b.Value;
    }

    // 小于操作符重载
    public static bool operator <(RefInt a, RefInt b)
    {
        return a.Value < b.Value;
    }

    // 大于等于操作符重载
    public static bool operator >=(RefInt a, RefInt b)
    {
        return a.Value >= b.Value;
    }

    // 小于等于操作符重载
    public static bool operator <=(RefInt a, RefInt b)
    {
        return a.Value <= b.Value;
    }
}
}