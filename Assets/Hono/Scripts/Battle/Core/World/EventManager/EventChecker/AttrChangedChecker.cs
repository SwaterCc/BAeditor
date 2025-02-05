using System;

namespace Hono.Scripts.Battle.Event
{
    [Serializable]
    public class AttrChangedChecker : IEventChecker
    {
        public EAttrType checkAttrType;
        public bool Check(in VariableBoard board)
        {
            return board.Get(AttrChangedEventInfo.AttrType) == checkAttrType;
        }
    }
}