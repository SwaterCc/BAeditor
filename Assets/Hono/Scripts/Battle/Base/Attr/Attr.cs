namespace Hono.Scripts.Battle.Base
{
    /// <summary>
    ///  属性基类
    /// </summary>
    public class Attr : IAPoolObject
    {
        private int _dynamicValue;
        private int _permanentValue;
        private RefInt _finalValue = new();
        
        /// <summary>
        /// 获取属性值（指令值和原值的总值）
        /// </summary>
        /// <returns></returns>
        public int Get()
        {
            return _finalValue.Value;
        }

        /// <summary>
        /// 获取引用最终值
        /// </summary>
        /// <returns></returns>
        public RefInt GetRef()
        {
            return _finalValue;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isPermanent"></param>
        public void Set(int value, bool isPermanent)
        {
            if (isPermanent)
            {
                _dynamicValue += value;
            }
            else
            {
                _permanentValue = value;
            }

            _finalValue = _dynamicValue + _permanentValue;
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

        public void OnRecycle()
        {
            _finalValue = 0;
            _dynamicValue = 0;
            _permanentValue = 0;
        }
    }
}