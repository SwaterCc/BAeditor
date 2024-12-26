#region

using System;

#endregion

namespace Hono.Scripts.Battle.Base
{
    [Serializable]
    public class RefBoolean : ARef, IAPoolObject
    {
        public bool Value;
        private int _refCount;

        public RefBoolean() { }

        public RefBoolean(bool initialValue = false)
        {
            Value = initialValue;
        }

        public void OnRecycle()
        {
            Value = false;
        }

        public override Type GetValueType()
        {
            return typeof(bool);
        }
        
        public override ARef DeepCopy()
        {
            RefBoolean rInt = APool<RefBoolean>.Pool.Rent();
            rInt.Value = Value;
            return rInt;
        }

        public override void ARefRecycle()
        {
            APool<RefBoolean>.Pool.Recycle(this);
        }

        // 隐式转换到 bool
        public static implicit operator bool(RefBoolean refBoolean)
        {
            return refBoolean.Value;
        }

        // 等于操作符重载
        public static bool operator ==(RefBoolean a, RefBoolean b)
        {
            return a.Value == b.Value;
        }

        // 不等于操作符重载
        public static bool operator !=(RefBoolean a, RefBoolean b)
        {
            return a.Value != b.Value;
        }

        private new bool Equals(object obj)
        {
            if (!(obj is RefBoolean)) return false;
            return this == (RefBoolean)obj;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}