using System;
using Hono.Scripts.Battle.Core;

namespace Hono.Scripts.Battle.Event
{
    public class AttrChangedEventChecker : IEventChecker
    {
        private int _sourceUnitUid;
        private EAttrType _checkAttrType;

        public AttrChangedEventChecker() { }

        public AttrChangedEventChecker(int sourceUnitUid, EAttrType checkAttrType)
        {
            _sourceUnitUid = sourceUnitUid;
            _checkAttrType = checkAttrType;
        }

        public void OnRent(int sourceUnitUid, EAttrType checkAttrType)
        {
            _sourceUnitUid = sourceUnitUid;
            _checkAttrType = checkAttrType;
        }

        public bool Check(in VariableBoard board)
        {
            var res = board.GetEvtField(AttrChangedEventInfo.SourceUnitUid) == _sourceUnitUid;
            res = res && board.GetEvtField(AttrChangedEventInfo.AttrType) == _checkAttrType;
            return res;
        }

        public void OnRecycle()
        {
            _sourceUnitUid = 0;
            _checkAttrType = 0;
        }
    }
}