using System;

namespace Hono.Scripts.Battle.Event
{
    public class MonsterGenRtChecker : EventChecker
    {
        private int _mGenUid;
        private int _configId;
        private int _roundCount;

        public MonsterGenRtChecker(EBattleEventType eventType, int uid, int configId, int round,
            Action<IEventInfo> func = null) : base(eventType,
            BattleConstValue.BattleRootControllerUid, func)
        {
            _mGenUid = uid;
            _configId = configId;
            _roundCount = round;
        }

        protected override bool onCheck(IEventInfo info)
        {
            var rtInfo = (MonsterGenRtEventInfo)info;
            var res = true;
            if (_mGenUid > 0)
            {
                res = _mGenUid == rtInfo.MonsterGeneratorUid;
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