namespace Hono.Scripts.Battle.Base
{
    /// <summary>
    ///  属性基类
    /// </summary>
    public class Attr : IAPoolObject
    {
        private int _commandTotalValue;
        private int _originValue;

        /// <summary>
        /// 获取属性值（指令值和原值的总值）
        /// </summary>
        /// <returns></returns>
        public int Get()
        {
            return _commandTotalValue + _originValue;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isCommand"></param>
        public void Set(int value, bool isCommand)
        {
            if (isCommand)
            {
                _commandTotalValue += value;
            }
            else
            {
                _originValue = value;
            }
        }

        #region 运算符重写

        // 隐式转换到 int
        public static implicit operator int(Attr attr)
        {
            return attr.Get();
        }

        // 会产生池对象
        public static explicit operator RefInt(Attr attr)
        {
            RefInt refInt = AObjectPool<RefInt>.Pool.Rent();
            refInt.Value = attr.Get();
            return refInt;
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
            _commandTotalValue = 0;
            _originValue = 0;
        }
    }
}