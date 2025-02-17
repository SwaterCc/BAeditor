using System;
using Hono.Scripts.Battle.Core;

namespace Hono.Scripts.Battle.Event
{
    [Serializable]
    public class AttrChangedChecker : IEventChecker
    {
        public int sourceUnitUid;
        public EAttrType checkAttrType;
        public bool Check(in VariableBoard board)
        {
            var res = board.Get(AttrChangedEventInfo.SourceUnitUid) == sourceUnitUid;
            res = res && board.Get(AttrChangedEventInfo.AttrType) == checkAttrType;
            return res;
        }
    }
}