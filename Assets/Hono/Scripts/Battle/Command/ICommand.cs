using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle
{
    public interface ICommand
    {
        void Do();
        void Undo();
    }

    /// <summary>
    ///     属性改变记录，目前仅记录值类型修改，引用类型的修改需要自己单独继承实现
    /// </summary>
    public struct AttrCommand : ICommand
    {
        /// <summary>
        /// 修改的值
        /// </summary>
        private readonly int _value;

        /// <summary>
        /// 绑定的Attr
        /// </summary>
        private readonly Attr _attr;

        public AttrCommand(Attr attr, int value)
        {
            _attr = attr;
            _value = value;
        }

        public void Do()
        {
            _attr.Set(_value, true);
        }

        public void Undo()
        {
            _attr.Set(-_value, true);
        }
    }
}