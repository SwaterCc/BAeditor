using System;
using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle.Event
{
    public class MonsterGenRtChecker : EventChecker
    {
        public int _mGenUid;
        public int _configId;
        public int _roundCount;

        public MonsterGenRtChecker() : base(EEventType.OnCallMonsterGenerator) { }

        protected override bool onCheck(in VariableBoard board)
        {
            var res = true;
            if (_mGenUid > 0)
            {
                res = _mGenUid == board.Get<int>("MonsterGeneratorUid");
            }

            if (_configId > 0)
            {
                res = res && _configId == rtInfo.ConfigId;
            }

            if (_roundCount >= 0)
            {
                res = res && _roundCount == rtInfo.CurRoundCount;
            }

            return res;
        }
    }
}