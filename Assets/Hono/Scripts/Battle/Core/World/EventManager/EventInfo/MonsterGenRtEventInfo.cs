namespace Hono.Scripts.Battle.Event
{
    public class MonsterGenRtEventInfo
    {
        public static readonly EvtInfoField<int> MonsterGeneratorUid = new();
        public static readonly EvtInfoField<int> ConfigId = new();
        public static readonly EvtInfoField<int> CurRoundCount = new();
    }
}