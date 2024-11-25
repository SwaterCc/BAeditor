#region

using System;

#endregion

namespace Hono.Scripts.Battle.Base
{
    [Serializable]
    public class RefBool : ARef, IAPoolObject
    {
        public bool Value;
        private int _refCount;

        public RefBool() { }

        public RefBool(bool initialValue = false)
        {
            Value = initialValue;
        }

        public void OnRecycle()
        {
            Value = false;
        }

        public override ARef DeepCopy()
        {
            RefBool rInt = APool<RefBool>.Pool.Rent();
            rInt.Value = Value;
            return rInt;
        }

        public override void ARefRecycle()
        {
            APool<RefBool>.Pool.Recycle(this);
        }

        // 隐式转换到 bool
        public static implicit operator bool(RefBool refBool)
        {
            return refBool.Value;
        }

        // 等于操作符重载
        public static bool operator ==(RefBool a, RefBool b)
        {
            return a.Value == b.Value;
        }

        // 不等于操作符重载
        public static bool operator !=(RefBool a, RefBool b)
        {
            return a.Value != b.Value;
        }

        private new bool Equals(object obj)
        {
            if (!(obj is RefBool)) return false;
            return this == (RefBool)obj;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}