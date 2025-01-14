using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// Attr指令
    /// </summary>
    public class AttrCommand : ICommand
    {
        /// <summary>
        /// 修改的值
        /// </summary>
        private int _value;

        /// <summary>
        /// 绑定的Attr
        /// </summary>
        private Attr _attr;

        public AttrCommand() { }

        public void SetAttr(Actor actor, EAttrType attrType, int value)
        {
            _attr = actor.Attrs.GetAttr(attrType);
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

        public void OnRecycle()
        {
            _attr = null;
            _value = 0;
        }
    }
}