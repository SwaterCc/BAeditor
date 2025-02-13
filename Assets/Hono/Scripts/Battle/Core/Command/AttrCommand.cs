namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// Attr指令
    /// </summary>
    public struct AttrCommand : ICommand
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
    }
}