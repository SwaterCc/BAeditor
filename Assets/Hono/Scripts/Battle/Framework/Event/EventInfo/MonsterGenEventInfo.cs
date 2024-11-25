#region

using System.Collections.Generic;

#endregion

namespace Hono.Scripts.Battle.Event
{
    public class MonsterGenEventInfo : IEventInfo
    {
        public bool FiredAll;
        public int SingleUid;
        public List<int> SpecialUids = new();
        public int MonsterConfigId;
        public EMonsterGenBehave Behave;

        public void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}