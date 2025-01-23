using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Core;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// Attr指令
    /// </summary>
    public class AttrCommand : ICommand
    {
        private int _value;
        private EAttrType _attrType;
        private Unit _unit;

        public void InitCommand(Unit unit, EAttrType attrType, int value)
        {
            _unit = unit;
            _attrType = attrType;
            _value = value;
        }

        public void Do()
        {
            _unit.SetAttr(_attrType, _value);
        }

        public void Undo()
        {
            _unit.SetAttr(_attrType, -_value);
        }

        public void OnRecycle()
        {
            _unit = null;
            _attrType = 0;
            _value = 0;
        }
    }
}