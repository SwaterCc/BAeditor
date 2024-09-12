namespace Hono.Scripts.Battle.RefValue
{
    public class RefBool : RefValue<bool>
    {
        public RefBool(bool initialValue = false) : base(initialValue) { }

        // 隐式转换到 bool
        public static implicit operator bool(RefBool refBool)
        {
            return refBool.Value;
        }

        // 隐式转换从 bool
        public static implicit operator RefBool(bool value)
        {
            return new RefBool(value);
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

        public override bool Equals(object obj)
        {
            if (!(obj is RefBool)) return false;
            return this == (RefBool)obj;
        }
    }
}