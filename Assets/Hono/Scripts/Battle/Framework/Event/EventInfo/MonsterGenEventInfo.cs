using System.Collections.Generic;

namespace Hono.Scripts.Battle.Event
{
    public class MonsterGenEventInfo : IEventInfo
    {
        public bool FiredAll;
        public int SingleUid;
        public List<int> SpecialUids = new();
        public int MonsterConfigId;
    }
}