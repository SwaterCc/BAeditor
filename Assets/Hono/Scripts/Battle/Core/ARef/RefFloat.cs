#region

using System;
using System.Globalization;

#endregion

namespace Hono.Scripts.Battle.Base
{
    [Serializable]
    public class RefFloat : ARef, IAPoolObject
    {
        public float Value;

        public RefFloat()
        {
            Value = 0;
        }

        public RefFloat(float initialValue = 0f)
        {
            Value = initialValue;
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override Type GetValueType()
        {
            return typeof(float);
        }
        
        // 隐式转换到 float
        public static implicit operator float(RefFloat refFloat)
        {
            return refFloat.Value;
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

        public override ARef DeepCopy()
        {
            RefFloat rInt = APool<RefFloat>.Pool.Rent();
            rInt.Value = Value;
            return rInt;
        }

        public override void ARefRecycle()
        {
            APool<RefFloat>.Pool.Recycle(this);
        }

        public void OnRecycle()
        {
            Value = 0;
        }
    }
}