#region

using System;

#endregion

namespace Hono.Scripts.Battle.Base
{
    [Serializable]
    public class RefInt : ARef, ICPoolObject
    {
        public int Value;

        private int _refCount;

        public RefInt()
        {
            Value = 0;
            _refCount = 0;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public RefInt(int initialValue = 0)
        {
            Value = initialValue;
        }

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

        public override ARef DeepCopy()
        {
            RefInt rInt = GPool<RefInt>.Pool.Rent();
            rInt.Value = Value;
            return rInt;
        }

        public override Type GetValueType()
        {
            return typeof(int);
        }

        public override void ARefRecycle()
        {
            GPool<RefInt>.Pool.Recycle(this);
        }

        public void OnRecycle()
        {
            Value = 0;
            _refCount = 0;
        }
    }
}