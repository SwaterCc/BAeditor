#region

using System.Collections.Generic;

#endregion

namespace Hono.Scripts.Battle.Event
{
    public class MonsterGenEventInfo
    {
        public static readonly EvtInfoField<bool> FiredAll = new();
        public static readonly EvtInfoField<int> SingleUid = new();
        public static readonly EvtInfoField<List<int>> SpecialUids = new();
        public static readonly EvtInfoField<int> MonsterConfigId = new();
        public static readonly EvtInfoField<EMonsterGenBehave> Behave = new();
    }
}