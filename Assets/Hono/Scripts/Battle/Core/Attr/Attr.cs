namespace Hono.Scripts.Battle.Base
{
    /// <summary>
    ///  属性
    /// </summary>
    public class Attr :IAPoolObject
    {
        private int _value;
        
        /// <summary>
        /// 获取属性值（指令值和原值的总值）
        /// </summary>
        /// <returns></returns>
        public int Get()
        {
            return _value;
        }

        /// <summary>
        /// 设置属性，如果属性值发生变化，则会设置脏标记，可以通过参数强制设置脏标记
        /// </summary>
        /// <param name="value"></param>
        /// <param name="forceDirty">当属性值并未发送数值上的变化时也可以触发脏标记</param>
        public bool Set(int value, bool forceDirty = false)
        {
            bool isDirty = value != _value || forceDirty;
            _value = value;
            return isDirty;
        }
        
        public void OnRecycle()
        {
            _value = 0;
        }
        
        #region 运算符重写

        // 隐式转换到 int
        public static implicit operator int(Attr attr)
        {
            return attr.Get();
        }

        // 加法操作符重载
        public static int operator +(Attr a, Attr b)
        {
            return a.Get() + b.Get();
        }

        // 减法操作符重载
        public static int operator -(Attr a, Attr b)
        {
            return a.Get() - b.Get();
        }

        // 乘法操作符重载
        public static int operator *(Attr a, Attr b)
        {
            return a.Get() * b.Get();
        }

        // 除法操作符重载
        public static int operator /(Attr a, Attr b)
        {
            if (b.Get() == 0) throw new System.DivideByZeroException();
            return a.Get() / b.Get();
        }

        // 大于操作符重载
        public static bool operator >(Attr a, Attr b)
        {
            return a.Get() > b.Get();
        }

        // 小于操作符重载
        public static bool operator <(Attr a, Attr b)
        {
            return a.Get() < b.Get();
        }

        // 大于等于操作符重载
        public static bool operator >=(Attr a, Attr b)
        {
            return a.Get() >= b.Get();
        }

        // 小于等于操作符重载
        public static bool operator <=(Attr a, Attr b)
        {
            return a.Get() <= b.Get();
        }

        #endregion
    }
}